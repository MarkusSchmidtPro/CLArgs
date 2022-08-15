using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

[PublicAPI]
public class Commander2
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Commander2> _logger;



    public Commander2(IServiceProvider serviceProvider, ILogger<Commander2> logger)
    {
        _scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        _logger     = logger;
    }



    public void Execute()
    {
        using IServiceScope commandScope = _scopeFactory.CreateScope();
        IServiceProvider scopeServiceProvider =  commandScope.ServiceProvider;
        
        ICommandDescriptorCollection commandDescriptors = scopeServiceProvider.GetRequiredService<ICommandDescriptorCollection>();
        if (commandDescriptors == null || commandDescriptors.Count == 0)
            throw new ApplicationException("No Commands have been registered");


        foreach (var descriptor in commandDescriptors)
        {
            _logger.LogDebug("'{Verb}' -> {Type}", descriptor.Key, descriptor.Value.Type);
        }
        
        IArgumentCollection clArgs = scopeServiceProvider.GetRequiredService<IArgumentCollection>();
        foreach (var arg in clArgs)
        {
            _logger.LogDebug("{Key}:{Value} ({Type})", arg.Key, arg.Value, arg.Type);
        }

        if (clArgs.Count == 0)
        {
            IHelpBuilder hb = scopeServiceProvider.GetRequiredService<IHelpBuilder>();
            hb.BuildAllCommandsHelp();
            return;
        }

        if (clArgs.VerbPath == null)
            throw new ApplicationException("No Verb provided!");

        if (!commandDescriptors.ContainsKey(clArgs.VerbPath))
            throw new ApplicationException($"No command registered for Verb: '{clArgs.VerbPath}'");

        CommandDescriptor2 commandDescriptor = commandDescriptors[clArgs.VerbPath];
        
        if (clArgs.Options.Any(o => o.Key.Equals( "?") || o.Key == "help"))
        {
            IHelpBuilder hb = scopeServiceProvider.GetRequiredService<IHelpBuilder>();
            hb.BuildCommandHelp(commandDescriptor);
            return;
        }
       
        ICommand2 command = (ICommand2)scopeServiceProvider.GetRequiredService(commandDescriptor.Type);
        command.Execute();
    }
}