using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

public class CommandHost : IHost
{
    private readonly ILogger<CommandHost> _logger;



    public CommandHost(IServiceProvider serviceProvider,ILogger<CommandHost> logger)
    {
        this.Services = serviceProvider;
        _logger       = logger;
    }



    private void execute()
    {
        CommandLineParser2 cp = Services.GetRequiredService<CommandLineParser2>();
        IArgumentCollection arguments = Services.GetRequiredService<IArgumentCollection>();
        cp.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray(), arguments);
        
        
        
        
        ICommandDescriptorCollection commandDescriptors =
            Services.GetRequiredService<ICommandDescriptorCollection>();
        if (commandDescriptors == null || commandDescriptors.Count == 0)
            throw new ApplicationException("No Commands have been registered");


        foreach (var descriptor in commandDescriptors)
        {
            _logger.LogDebug("'{Verb}'->{Type}", descriptor.Key, descriptor.Value.Type);
        }

        IArgumentCollection clArgs = Services.GetRequiredService<IArgumentCollection>();
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
            IHelpBuilder hb = Services.GetRequiredService<IHelpBuilder>();
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
            IHelpBuilder hb = Services.GetRequiredService<IHelpBuilder>();
            hb.BuildCommandHelp(commandDescriptor);
            return;
        }

        ICommand2 command = (ICommand2)Services.GetRequiredService(commandDescriptor.Type);
        command.Execute();
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



    public Task StopAsync(CancellationToken cancellationToken = new())
    {
        return _task;
    }



    public IServiceProvider Services { get; }

    #endregion
}