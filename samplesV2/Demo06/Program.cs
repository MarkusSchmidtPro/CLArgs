using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;



Console.WriteLine("Demo06 - Context Sets and argument files");

// string[] commandline = "API /url http://localhost /username markus /password schmidt".Split(" ").ToArray();

// Use argument file
//string[] commandline = "API /p1=p1111 @devsystem.profile".Split(" ").ToArray();
string[] commandline = "API /?".Split(" ").ToArray();

HostApplicationBuilder builder = Host.CreateApplicationBuilder();
builder.ConfigureCommands(commandline);
builder.Build().Start();