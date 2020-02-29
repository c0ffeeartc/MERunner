using System;
using CommandLine;

namespace MERunner
{
	public class Args
	{
		[Option( "SettingsPath", Required = true, HelpText = "Path to settings file" )]
		public String SettingsPath { get; set; }
	}

	internal class Program
	{
		public static void Main( string[] args )
		{
			Parser.Default.ParseArguments<Args>(args).WithParsed( Run );
		}

		public static void Run( Args args )
		{
			Hub.SettingsGrammar	= new SettingsGrammar(  );
			var bootstrap		= new Bootstrap(  );
			bootstrap.Run( args.SettingsPath );

			var systems			= bootstrap.Systems;

			systems.Initialize(  );
			systems.Execute(  );
			systems.Cleanup(  );
			systems.TearDown(  );
		}
	}
}