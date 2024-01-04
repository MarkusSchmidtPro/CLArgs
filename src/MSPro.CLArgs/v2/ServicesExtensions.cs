using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs
{
    
    public static class ServicesExtensions
    {
        public static IServiceCollection AddCLArgsServices(this IServiceCollection services)
        {
            // This is required to have a default logger,
            // so that ILogger does not resolve to null
            //services.AddLogging(configure => configure.AddConsole());
        
            services.AddScoped<IPrinter, ConsolePrinter>();
            services.AddScoped<IHelpBuilder, HelpBuilder>();

            services.AddScoped<ContextPropertyResolver>();
            services.AddScoped<ContextBuilder>();
            services.AddScoped<CommandLineParser2>();
        
            return services;
        }
    }
} 