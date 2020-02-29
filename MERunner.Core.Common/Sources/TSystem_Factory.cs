using System;
using Entitas;

namespace MERunner
{
public abstract class TSystem_Factory<T>
		: ISystem_Factory
		where T : ISystem
{
	public					ISystem					Create					( Contexts contexts )
	{
		return (T)Activator.CreateInstance( typeof(T), contexts );
	}
}

public interface ISystem_Factory
{
	ISystem Create( Contexts contexts );
}
}
