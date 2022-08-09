using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace MSPro.CLArgs;

/// <summary>
/// </summary>
public class CommandDescriptor2
{
    /// <summary>
    /// </summary>
    public CommandDescriptor2([NotNull] string verb, [NotNull] Type commandType, string description = null)
    {
        Verb = verb;
        Type = commandType;
        Description = description;

        Debug.Assert( commandType.GetInterface(nameof(ICommand2)) != null, $"The provided Command type {commandType.Name} does not implement ICommand2");
    }


    /// <summary>
    /// </summary>
    public string Verb { get; }

    /// <summary>
    /// The type that implements the ICommand2 interface.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// </summary>
    public string Description { get; }
}