using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generic;

public interface Main : IScope{}

namespace MERunner
{
public struct SystemGuids : IComponent
		, ICompData
		, Scope<Settings>
		, IUnique
{
	public					List<Guid>			Values;

	public SystemGuids( List<Guid> values)
	{
		Values = values;
	}
}

public struct SystemsImported : IComponent
		, ICompData
		, Scope<Main>
		, IUnique
{
	public					List<ISystem>		Values;

	public SystemsImported( List<ISystem> values)
	{
		Values = values;
	}
}

public struct SystemsOrdered : IComponent
		, ICompData
		, Scope<Main>
		, IUnique
{
	public					List<ISystem>		Values;

	public SystemsOrdered( List<ISystem> values)
	{
		Values = values;
	}
}

}