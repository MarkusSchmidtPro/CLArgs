using System;
using System.Collections.Generic;

namespace MSPro.CLArgs;

public interface IArgumentConvertersCollection : IDictionary<Type, IArgumentConverter>
{
}