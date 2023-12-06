using System;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs.BuildInCommands
{
    [Command("VERB1")]
    public class MyCommand : ICommand2
    {
        private readonly ILogger<MyCommand> _logger;
        private readonly IServiceProvider _serviceProvider;



        public MyCommand(IServiceProvider serviceProvider, ILogger<MyCommand> logger)
        {
            _serviceProvider = serviceProvider;
            _logger          = logger;
            _logger.LogInformation("Create Instance");
        }



        void ICommand2.Execute()
        {
            _logger.LogInformation("MyCommand execute!");
        }
    }
}