﻿// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;
using SampleCommands.Services;



Console.WriteLine("Demo07 - How to use dependency injection in your commands");

string[] commandline = "HelloWorld /UserName Markus /Color red".Split(" ").ToArray();

var builder = CommandHostBuilder.Create(commandline);
// Register the IHelloWorldService Assembly to look for commands
builder.ConfigureCommands(collection => collection.AddAssembly(typeof(IHelloWorldService)));
// Use the extension method to register all Assembly services
// YOu can seamlessly use your services in all your Commands: see HelloWorldCommand
builder.ConfigureServices(services => services.AddSampleCommandServices());
builder.Start();



[Command("HelloWorld", "Say hello.")]
public class HelloWorldCommand : CommandBase2<HelloWorldContext>
{
    private readonly IHelloWorldService _helloWorldService;



    public HelloWorldCommand(IServiceProvider serviceProvider, IHelloWorldService helloWorldService) : base(serviceProvider)
    {
        _helloWorldService = helloWorldService;
    }



    protected override void Execute()
    {
        _helloWorldService.SayHello(this.Context.Username, this.Context.Color);
    }
}



public class HelloWorldContext
{
    [OptionDescriptor("Username", required: true, helpText: "Provide a username.")]
    public string Username { get; set; }

    [OptionDescriptor("Color", helpText: "Chose a color.", required: false)]
    public ColorType Color { get; set; }
}

