using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs
{
    
    public static class ServicesExtensions
    {
        public static void AddCLArgsServices(this IServiceCollection services)
        {
            //services.AddScoped<IPrinter, ConsolePrinter>();
            services.AddScoped<IHelpBuilder, HelpBuilder>();

            services.AddScoped<ContextPropertyResolver>();
            services.AddScoped<ContextBuilder>();
            services.AddScoped<CommandLineParser2>();
        }
    }
} 