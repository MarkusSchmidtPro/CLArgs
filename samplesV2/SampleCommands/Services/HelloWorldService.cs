using MSPro.CLArgs;



namespace SampleCommands.Services;

/// <summary>
/// A service prepared for dependency injection.
/// </summary>
/// <remarks>
/// Add all your assembly services to <see cref="AssemblyServices.AddSampleCommandServices"/>.
/// </remarks>
public class HelloWorldService() : IHelloWorldService
{
    public void SayHello(string username, ColorType color)
    {
        Console.WriteLine($"Hello World, hello {username}");
        Console.WriteLine($"Your color is: {color}");
    }
}