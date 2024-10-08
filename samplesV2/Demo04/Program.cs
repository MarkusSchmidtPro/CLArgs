﻿// See https://aka.ms/new-console-template for more information


using Demo04;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;



Console.WriteLine("Demo04 - Argument conversion and validation");

string[] commandline = "ARGS DEMO4 /d c:\\windows".Split(" ").ToArray();

try
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder();
    builder.ConfigureCommands(commandline, commandDescriptors => commandDescriptors.AddAssembly(typeof(Demo1Command)));
    IHost host = builder.Build();

    // To build a Context from the provided commend-line arguments
    // it requires the ContextBuilder which we are going to extend
    ContextBuilder cb = host.Services.GetRequiredService<ContextBuilder>();
    cb.ConfigureConverters(converters =>
        converters.AddCustomConverter(typeof(DirectoryInfo), new DirectoryInfoConverter()));
    // The ContextBuilder.Build() is called implicitly 
    // right before the Command executes.
    host.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}