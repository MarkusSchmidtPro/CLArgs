using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

public class Commander2
{
    private readonly ILogger<Commander2> _logger;



    public Commander2(IServiceProvider serviceProvider, ILogger<Commander2> logger)
    {
        this.ServiceProvider = serviceProvider;

        //IServiceScope commandScope = serviceProvider.CreateScope();
        //this.ServiceProvider = commandScope.ServiceProvider;
        _logger = logger;
    }



    private IServiceProvider ServiceProvider { get; }



    public void Execute()
    {
        ICommandDescriptorCollection commandDescriptors = this.ServiceProvider.GetRequiredService<ICommandDescriptorCollection>();
        if (commandDescriptors == null || commandDescriptors.Count == 0)
            throw new ApplicationException("No Commands have been registered");


        foreach (var descriptor in commandDescriptors)
        {
            _logger.LogDebug("'{Verb}'->{Type}", descriptor.Key, descriptor.Value.Type);
        }

        IArgumentCollection clArgs = this.ServiceProvider.GetRequiredService<IArgumentCollection>();
        foreach (var arg in clArgs)
        {
            switch (arg.Type)
            {
                case ArgumentType.Verb:
                    _logger.LogDebug($"{arg.Type}:{arg.Key}");
                    break;
                case ArgumentType.Option:
                    _logger.LogDebug($"{arg.Type}:{arg.Key}={arg.Value}");
                    break;
                case ArgumentType.Target:
                    _logger.LogDebug($"{arg.Type}:{arg.Value}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (clArgs.Count == 0)
        {
            IHelpBuilder hb = this.ServiceProvider.GetRequiredService<IHelpBuilder>();
            Console.WriteLine(hb.BuildAllCommandsHelp());
            return;
        }

        CommandDescriptor2 commandDescriptor;
        if (clArgs.VerbPath == null)
        {
            if (commandDescriptors.Count > 1)
                throw new ApplicationException("No Verb provided!");

            commandDescriptor = commandDescriptors.First().Value;
        }
        else
        {
            if (!commandDescriptors.ContainsKey(clArgs.VerbPath))
                throw new ApplicationException($"No command registered for Verb: '{clArgs.VerbPath}'");

            commandDescriptor = commandDescriptors[clArgs.VerbPath];
        }

        if (clArgs.Options.Any(o => o.Key.Equals("?") || o.Key == "help"))
        {
            IHelpBuilder hb = this.ServiceProvider.GetRequiredService<IHelpBuilder>();
            hb.BuildCommandHelp(commandDescriptor);
            return;
        }

        ICommand2 command = (ICommand2)this.ServiceProvider.GetRequiredService(commandDescriptor.Type);
        command.Execute();
    }
}