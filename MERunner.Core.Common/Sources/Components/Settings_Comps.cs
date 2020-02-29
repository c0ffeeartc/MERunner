using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;

public interface Settings : IScope{}

namespace MERunner
{
public struct SettingsPath : IComponent, ICompData
		, Scope<Settings>
		, IUnique
{
	public					String					Value;

	public SettingsPath( String value)
	{
		Value = value;
	}
}

public struct SettingsParseInput : IComponent, ICompData
		, Scope<Settings>
		, IUnique
{
	public					String					Value;

	public SettingsParseInput( String value)
	{
		Value = value;
	}
}

public struct SettingsDict : IComponent, ICompData
		, Scope<Settings>
		, IUnique
{
	public			Dictionary<String,List<String>>	Dict;

	public SettingsDict( Dictionary<String, List<String>> dict)
	{
		Dict = dict;
	}
}
}