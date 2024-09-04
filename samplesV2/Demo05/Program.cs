using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSPro.CLArgs;
using NLog.Extensions.Logging;



Console.WriteLine("Demo05 - Logging and Printing");

//string[] commandline = "MATH MULT2 /f1=5,43 /f2=7,54 ".Split(" ").ToArray();
string[] commandline = "MATH MULT3 /f1=5,43 /f2=7,54 ".Split(" ").ToArray();

try
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder();
    builder.ConfigureCommands(commandline);

    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
    });
    builder.Build().Start();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}