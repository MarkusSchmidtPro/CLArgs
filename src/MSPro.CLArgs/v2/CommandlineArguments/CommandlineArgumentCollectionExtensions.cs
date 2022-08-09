using System.Linq;
using JetBrains.Annotations;

namespace MSPro.CLArgs;

[PublicAPI]
public static class CommandlineArgumentCollectionExtensions
{
    public static ICommandlineArgumentCollection AddArguments(this ICommandlineArgumentCollection commandlineArguments,
        string[] args, Settings2 settings)
    {
        var cp =  CommandLineParser.Parse(args.Skip(1).ToArray(),settings);
        foreach (string name in cp.Verbs)
        {
            commandlineArguments.Add( CommandlineArgument.Verb( name));
        } 
        
        foreach (var option in cp.Options)
        {
            commandlineArguments.Add( CommandlineArgument.Option( option.Key, option.Value));
        }     
        
        foreach (var target in cp.Targets)
        {
            commandlineArguments.Add( CommandlineArgument.Target( target));
        }

        return commandlineArguments;
    }
}