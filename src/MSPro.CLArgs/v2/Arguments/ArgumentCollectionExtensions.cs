
using System.Collections.Generic;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

[PublicAPI]
public static class ArgumentCollectionExtensions
{
    public static IArgumentCollection AddRange(this IArgumentCollection arguments, IEnumerable<Argument> range)
    {
        foreach (var item in range)
        {
            arguments.Add(item);
        }
        return arguments;
    }
}