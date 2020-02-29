using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Entitas;

namespace MERunner
{
	public class MefImportSystems
	{
		[ImportMany(typeof(ISystem_Factory))]
		private				IEnumerable<ISystem_Factory>	_imports;

		public				void					Import					(  )
		{
			var catalog				= new DirectoryCatalog( ".", "MERunner.*.dll" );
			var composition			= new CompositionContainer( catalog );
			composition.ComposeParts( this );
		}

		public				List<ISystem>			CreateSystems			( Contexts contexts )
		{
			var systems				= new List<ISystem>(  );
			foreach ( var factory in _imports )
			{
				var system			= factory.Create( contexts );
				systems.Add( system );
			}
			systems.Sort( (a, b) => String.Compare( a.GetType(  ).Name, b.GetType(  ).Name, StringComparison.Ordinal ) );
			return systems;
		}
	}
}