using System;
using JetBrains.Annotations;

namespace MSPro.CLArgs;

/// <summary>
/// </summary>
public class CommandDescriptor
{
    /// <summary>
    /// </summary>
    public CommandDescriptor([NotNull] string verb, [NotNull] Func<ICommand> hostFactory, string description = null)
    {
        Verb = verb;
        CreateCommand = hostFactory;
        Description = description;
    }


    /// <summary>
    /// </summary>
    public string Verb { get; }

    /// <summary>
    /// </summary>
    public Func<ICommand> CreateCommand { get; }

    /// <summary>
    /// </summary>
    public string Description { get; }
}