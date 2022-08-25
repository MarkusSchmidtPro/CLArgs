using Microsoft.Extensions.DependencyInjection;



namespace SampleCommands.Services;

public static class AssemblyServices
{
    /// <summary>
    /// Register the services in this Assembly to be used with Dependency Injection. 
    /// </summary>
    /// <example>How to use:
    /// <code>
    /// var builder =CommandHostBuilder.Create(commandline);
    /// builder.ConfigureServices(services => services.AddSampleCommandServices());
    /// builder.Start();
    /// </code>
    /// </example>
    public static void AddSampleCommandServices( this IServiceCollection services)
    {
        services.AddScoped<IHelloWorldService, HelloWorldService>();
    }
}