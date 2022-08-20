using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MSPro.CLArgs.Print;



namespace MSPro.CLArgs;

[PublicAPI]
public static class ServicesExtensions
{
    public static IServiceCollection AddCLArgsServices(this IServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole());
        services.AddScoped<IPrinter, ConsolePrinter>();
        services.AddScoped<IHelpBuilder, HelpBuilder>();

        services.AddScoped<ArgumentOptionMapper>();
        services.AddScoped<ContextBuilder>();
        services.AddScoped<CommandLineParser2>();
        services.AddScoped<Commander2>();
        
        return services;
    }
} 