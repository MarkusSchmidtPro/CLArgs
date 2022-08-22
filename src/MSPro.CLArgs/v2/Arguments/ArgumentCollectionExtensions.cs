
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs;

[PublicAPI]
public static class ArgumentCollectionExtensions
{
    public static IArgumentCollection AddCommandLine(this IArgumentCollection arguments, string[] args, Settings2 settings2)
    {
        CommandLineParser2 cp = new (settings2);
        cp.Parse(args.Skip(1).ToArray(), arguments);
        return arguments;
    }
    
    public static IArgumentCollection AddRange(this IArgumentCollection arguments, IEnumerable<Argument> range)
    {
        foreach (var item in range)
        {
            arguments.Add(item);
        }
        return arguments;
    }
}