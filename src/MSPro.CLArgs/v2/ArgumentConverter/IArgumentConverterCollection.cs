using System;
using System.Collections.Generic;

namespace MSPro.CLArgs;

public interface IArgumentConverterCollection : IDictionary<Type, IArgumentConverter>
{
}