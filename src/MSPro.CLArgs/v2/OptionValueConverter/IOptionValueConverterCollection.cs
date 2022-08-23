using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs;

public interface IOptionValueConverterCollection : IDictionary<Type, IArgumentConverter>
{
    
}