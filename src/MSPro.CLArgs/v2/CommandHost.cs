using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

public class CommandHost(IServiceProvider serviceProvider, ILogger<CommandHost> logger) : IHost
{
    private void execute()
    {
        var commandDescriptors =
            Services.GetRequiredService<ICommandDescriptorCollection>();
        if (commandDescriptors == null || commandDescriptors.Count == 0)
            throw new ApplicationException("No Commands have been registered");


        foreach (KeyValuePair<string, CommandDescriptor2> descriptor in commandDescriptors)
        {
            logger.LogDebug("'{Verb}'->{Type}", descriptor.Key, descriptor.Value.Type);
        }

        var clArgs = Services.GetRequiredService<IArgumentCollection>();
        _logArguments(clArgs);
        if (clArgs.Count == 0  )
        {
            var hb = Services.GetRequiredService<IHelpBuilder>();
            Console.WriteLine(hb.BuildAllCommandsHelp());
            return;
        }

        // Get the implementing type for a given command name
        // by finding the name in the registered command descriptors.
        if (!commandDescriptors.TryGetValue(clArgs.VerbPath, out CommandDescriptor2 commandDescriptor))
            throw new ApplicationException($"No command registered for Verb: '{clArgs.VerbPath}'");

        if ( clArgs.Options.Any(o => o.Key.Equals("?") || o.Key == "help"))
        {
            var hb = Services.GetRequiredService<IHelpBuilder>();
            string buildCommandHelp = hb.BuildCommandHelp(commandDescriptor);
            Console.WriteLine(buildCommandHelp);
            return;
        }

        var command = (ICommand2)Services.GetRequiredService(commandDescriptor.Type);
        command.Execute();
    }



    private void _logArguments(IArgumentCollection clArgs)
    {
        foreach (Argument arg in clArgs)
        {
            switch (arg.Type)
            {
                case ArgumentType.Verb:
                    logger.LogDebug($"{arg.Type}:{arg.Key}");
                    break;
                case ArgumentType.Option:
                    logger.LogDebug($"{arg.Type}:{arg.Key}={arg.Value}");
                    break;
                case ArgumentType.Target:
                    logger.LogDebug($"{arg.Type}:{arg.Value}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }



    #region IHost Implementation

    private Task _task;



    public void Dispose()
    {
        if (_task == null) return;
        if (_task.Status == TaskStatus.RanToCompletion
            || _task.Status == TaskStatus.Faulted
            || _task.Status == TaskStatus.Canceled) _task.Dispose();

        _task = null;
    }



    public Task StartAsync(CancellationToken cancellationToken = new())
    {
        _task = Task.Run(() =>
        {
            execute();
            return Task.CompletedTask;
        }, cancellationToken);
        return _task;
    }



    public Task StopAsync(CancellationToken cancellationToken = new()) => _task;


    public IServiceProvider Services { get; } = serviceProvider;

    #endregion
}