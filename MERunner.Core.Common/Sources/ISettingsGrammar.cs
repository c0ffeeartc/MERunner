using System;
using System.Collections.Generic;

namespace MERunner
{
public interface ISettingsGrammar
{
	Dictionary<String,List<String>> Parse			( String str );
	Boolean					BoolFromStr				( String str );
}
}