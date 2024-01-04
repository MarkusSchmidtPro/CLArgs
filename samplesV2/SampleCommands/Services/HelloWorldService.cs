using MSPro.CLArgs;



namespace SampleCommands.Services;

/// <summary>
/// A service prepared for dependency injection.
/// </summary>
/// <remarks>
/// Add all your assembly services to <see cref="AssemblyServices.AddSampleCommandServices"/>.
/// </remarks>
public class HelloWorldService : IHelloWorldService
{
    private readonly IPrinter _printer;


    // The services expects IPrinter to be injected.
    
    public HelloWorldService(IPrinter printer)
    {
        _printer = printer;
    }



    public void SayHello(string username, ColorType color)
    {
        _printer.Info($"Hello World, hello {username}");
        _printer.Info($"Your color is: {color}");
    }
}