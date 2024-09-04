using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



namespace MSPro.CLArgs;

public static class HostApplicationBuilderExtensions //CommandBuilder
{
    public static HostApplicationBuilder ConfigureCommands(
        this HostApplicationBuilder builder,
        string[] args,
        Action<ICommandDescriptorCollection>? addCommandDescriptors=null,
        Action<Settings2>? configure=null,
        Action<IArgumentCollection>? addArguments=null
    )
    {
        builder.Services.AddCLArgsServices();
        
        //
        // Create default settings and let the client configure the settings object
        // before we add it to the services collection.
        //
        var settings = new Settings2 { IgnoreCase = true };
        configure?.Invoke( settings);
        builder.Services.AddScoped(_ =>settings);

        //builder.ConfigureCommandlineArguments((arguments, settings2) =>
        //{
        //    arguments.AddCommandLine(args, settings2);


        //
        // Build the default collection and let the client add more descriptions.
        //
        ICommandDescriptorCollection commandDescriptors  = new CommandDescriptorCollection(settings);
        commandDescriptors.AddAssembly(Assembly.GetEntryAssembly()!);
        addCommandDescriptors?.Invoke(commandDescriptors);
        builder.Services.AddScoped(_ => commandDescriptors);
        foreach (CommandDescriptor2 commandDescriptor in commandDescriptors.Values)
        {
            builder.Services.AddScoped(commandDescriptor.Type);
        }

        //
        // Initialize argument collection and ask client to update as needed.
        //
        IArgumentCollection arguments = new ArgumentCollection();
        CommandLineParser2 cp = new (settings);
        cp.Parse(args/*.Skip(1).ToArray()*/, arguments);
        addArguments?.Invoke(arguments);
        builder.Services.AddScoped(_ => arguments);



        builder.Services.AddScoped<IHost, CommandHost>();


        /*
        builder.Services.AddScoped(_ => arguments);


        foreach (Action<IServiceCollection, Settings2> action in _configureServicesActions)
        {
            action(services, settings);
        }

        // HostBuild returns the IHost instance
        services.AddScoped<IHost, CommandHost>();


        builder.Services.ConfigureDefaults(args);
        */
        return builder;
    }
}