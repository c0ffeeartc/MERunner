using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;

namespace MERunner
{
public struct AssemblyResolvePaths : IComponent
		, ICompData
		, Scope<Settings>
		, IUnique
{
	public					List<String>			Value;

	public AssemblyResolvePaths( List<String> value)
	{
		Value = value;
	}
}
}