using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

/// <summary>
///     The top level class to easily use 'CLArgs'.
/// </summary>
[PublicAPI]
public class Commander2
{
    private readonly ServiceProvider _serviceProvider;



    internal Commander2(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }



    public void Execute()
    {
        ICommandDescriptorCollection commandDescriptors = _serviceProvider.GetRequiredService<ICommandDescriptorCollection>();
        if (commandDescriptors == null || commandDescriptors.Count == 0)
            throw new ApplicationException("No Commands have been registered");

        IArgumentCollection clArgs = _serviceProvider.GetRequiredService<IArgumentCollection>();

        if (clArgs.Count == 0)
            // Display all commands help.
            return;

        if (clArgs.VerbPath == null)
            throw new ApplicationException("No Verb provided!");

        if (!commandDescriptors.ContainsKey(clArgs.VerbPath))
            throw new ApplicationException($"No command registered for Verb: '{clArgs.VerbPath}'");

        if (clArgs.Options.Any(o => o.Key.Equals( "?") || o.Key == "help"))
        {
            Console.WriteLine($"Display Help for {clArgs.VerbPath}");
            return;
        }

        CommandDescriptor2 commandDescriptor = commandDescriptors[clArgs.VerbPath];

        IServiceScopeFactory scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope commandScope = scopeFactory.CreateScope();
        ICommand2 command = (ICommand2)commandScope.ServiceProvider.GetService(commandDescriptor.Type);
        command.Execute();
    }
}