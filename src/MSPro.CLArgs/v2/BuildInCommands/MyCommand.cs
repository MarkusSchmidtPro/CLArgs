using System;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs.BuildInCommands;

[Command("VERB1")]
public class MyCommand : ICommand2
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MyCommand> _logger;



    public MyCommand(IServiceProvider serviceProvider, ILogger<MyCommand> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _logger.LogInformation("Create Instance");
    }



    public void Execute()
    {
        _logger.LogInformation("MyCommand execute!");
    }
}