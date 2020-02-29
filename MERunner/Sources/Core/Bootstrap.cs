using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Entitas;
using Entitas.Generic;

namespace MERunner
{
public class Bootstrap
{
	private					String					Settings_Path;
	private					String					Settings_ParseInput;
	private			Dictionary<String,List<String>>	Settings_Dict;

	private					List<String>			AssemblyResolvePaths;
	private					List<Guid>				SystemGuids;
	private					List<ISystem>			SystemsImported;
	private					List<ISystem>			SystemsOrdered;
	public					Systems					Systems					= new Systems(  );

	public					void					Run						( String settingsPath )
	{
		ReadSettingsDict( settingsPath );
		AssemblyResolve(  );
		ImportSystems(  );
		PutBootstrapDataToComps(  );
	}

	private					void					PutBootstrapDataToComps	(  )
	{
		var contexts				= Hub.Contexts;

		var settings				= contexts.Get<Settings>();
		settings.Replace_( new SettingsPath( Settings_Path ) );
		settings.Replace_( new SettingsParseInput( Settings_ParseInput ) );
		settings.Replace_( new SettingsDict( Settings_Dict ) );
		settings.Replace_( new AssemblyResolvePaths( AssemblyResolvePaths ) );
		settings.Replace_( new SystemGuids( SystemGuids ) );

		var main					= contexts.Get<Main>();
		main.Replace_( new SystemsImported( SystemsImported ) );
		main.Replace_( new SystemsOrdered( SystemsOrdered ) );
	}

	private					void					ReadSettingsDict		( String settingsPath )
	{
		Settings_Path				= settingsPath;
		if ( !File.Exists( settingsPath ) )
		{
			throw new FileNotFoundException( $"Settings file does not exist: '{settingsPath}'" );
		}

		Settings_ParseInput			= File.ReadAllText( settingsPath );
		Settings_Dict				= Hub.SettingsGrammar.Parse( Settings_ParseInput );
	}

	private					void					AssemblyResolve			(  )
	{
		AssemblyResolvePaths		=
			Settings_Dict.ContainsKey( nameof( AssemblyResolvePaths ) )
				? Settings_Dict[nameof( AssemblyResolvePaths )]
				: new List<string>(  );

		if ( AssemblyResolvePaths.Count == 0 )
		{
			return;
		}
		new AssemblyResolver( AssemblyResolvePaths.ToArray(  ) );
	}

	private					void					ImportSystems			(  )
	{
		SystemGuids					= new List<Guid>(  );
		if ( Settings_Dict.ContainsKey( nameof( MERunner.SystemGuids ) ) )
		{
			foreach ( var str in Settings_Dict[ nameof ( MERunner.SystemGuids )])
			{
				var guid	= new Guid( str );
				SystemGuids.Add( guid );
			}
		}

		var import					= new MefImportSystems(  );
		import.Import(  );

		InitContexts(  );

		SystemsImported				= import.CreateSystems( Hub.Contexts );

		Systems_Add(  );
	}

	private					void					InitContexts		(  )
	{
		Lookup_ScopeManager.RegisterAll(  );

		Hub.Contexts		= new Contexts(  );
		Hub.Contexts.AddScopedContexts(  );
	}

	private					void					Systems_Add			(  )
	{
		if ( SystemGuids.Count == 0 )
		{
			throw new Exception( "No GUIDs provided in setting `SystemGuids`" );
		}

		var systems				= GetSystemsOrdered(  );
		SystemsOrdered			= systems;
		foreach ( var system in systems )
		{
			Systems.Add( system );
		}
	}

	private				List<ISystem>			GetSystemsOrdered		(  )
	{
		var systems				= new List<ISystem>(  );

		var importedSystems		= SystemsImported;
		var importedGuids		= new List<Guid>(  );
		for ( var i = 0; i < importedSystems.Count; i++ )
		{
			var s				= importedSystems[i];
			var guidAttr		= (GuidAttribute)s.GetType(  ).GetCustomAttributes( typeof( GuidAttribute ), false ).FirstOrDefault(  );
			if ( guidAttr == null )
			{
				throw new Exception( "No GuidAttribute in system: " + s.GetType(  ).Name );
			}

			var guid			= new Guid( guidAttr.Value );
			importedGuids.Add( guid );
		}

		ThrowOnDuplicateGuid( importedSystems, importedGuids );

		var guids				= SystemGuids;
		for ( var i = 0; i < guids.Count; i++ )
		{
			var guid			= guids[i];
			var systemI			= importedGuids.FindIndex( g => g == guid );
			if ( systemI < 0 )
			{
				throw new Exception( "Not found system with GUID: " + guid );
			}
			systems.Add( importedSystems[systemI] );
		}

		return systems;
	}

	private				void					ThrowOnDuplicateGuid	( List<ISystem> systems, List<Guid> guids )
	{
		var guidSet				= new HashSet<Guid>(  );
		for ( var i = 0; i < guids.Count; i++ )
		{
			var guid			= guids[i];
			if ( !guidSet.Contains( guid ) )
			{
				guidSet.Add( guid );
				continue;
			}
			var sb				= new StringBuilder(  );
			for ( var j = 0; j < systems.Count; j++ )
			{
				if ( guid != guids[j] )
				{
					continue;
				}
				sb.Append( systems[j].GetType(  ).Name );
				sb.Append( "\n" );
			}
			throw new Exception( "Duplicate GUID found " + guid + " in systems: \n" + sb );
		}
	}
}
}