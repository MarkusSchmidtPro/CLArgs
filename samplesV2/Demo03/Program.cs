using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;
using SampleCommands.Math.Mult;



Console.WriteLine("Demo03 - Extended Math");

string[] commandline = "MATH MULT /f1=5,43 /f2=7,54 ".Split(" ").ToArray();
//string[] commandline = "MATH SUM /v=2 /Value=10 /VALUE=15 /v=3 /v=5,6,7 /v=1;2;3".Split(" ").ToArray();
//string[] commandline = "MATH ANALYZE /v=2 \"c:\\Windows\"".Split(" ").ToArray();

try
{
    var builder = CommandHostBuilder.Create(commandline);
    builder.Configure(settings =>
    {

        settings.IgnoreCase           = false;
        settings.IgnoreUnknownOptions = true;
    });
    // Add _all_ command implementations from Assembly
    builder.ConfigureCommands(commandDescriptors
                                  => commandDescriptors.AddAssembly(typeof(MultCommand)));
    builder.Start();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}