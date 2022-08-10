using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;

namespace MSPro.CLArgs;

/// <summary>
///     All arguments as they were provided in the command-line.
/// </summary>
/// <remarks>
///     An <c>Argument</c> is either a <see cref="Verbs" />
///     or an <see cref="Option" />.
/// </remarks>
[PublicAPI]
public class CommandLineArguments2
{
    internal CommandLineArguments2( Settings2 settings)
    {
        this.Verbs        = new HashSet<string>(settings.GetStringComparer());
        this.Targets      = new HashSet<string>(settings.GetStringComparer());
        this.Options      = new Dictionary<string,Option>(StringComparer.InvariantCulture);
    }

    /// <summary>
    ///     The list of verbs in the sequence order
    ///     as they were provided in the command-line.
    /// </summary>
    public HashSet<string> Verbs { get; }

    /// <summary>
    ///     The list of Targets in the sequence order
    ///     as they were provided in the command-line.
    /// </summary>
    public HashSet<string> Targets { get; }


    /// <summary>
    ///     A key-value list of all options provided in the command-line.
    /// </summary>
    /// <remarks>
    ///     All option values are <c>strings</c> in the first instance.
    ///     Conversion may happen later.
    /// </remarks>
    public Dictionary<string,Option> Options { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="verb"></param>
    public void AddVerb(string verb) => this.Verbs.Add(verb);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void AddTarget(string target) => this.Targets.Add(target);




    /// <inheritdoc cref="AddOption(MSPro.CLArgs.Option)" />
    public void AddOption(Option option)
    {
        this.Options["abc"]= new Option("111");
        this.Options["ABC"]= new Option("222");
        this.Options["abc"]= new Option("333");
        
        this.Options.Add(option.Key, option);
    }
}