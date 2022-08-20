using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MSPro.CLArgs.Print;



namespace MSPro.CLArgs;

[PublicAPI]
public class CommandBuilder
{
    private readonly List<Action<Settings2>> _configureArgumentConvertersActions = new();
    private readonly List<Action<IArgumentCollection, Settings2>> _configureArgumentsActions = new();
    private readonly List<Action<ICommandDescriptorCollection>> _configureCommandsActions = new();
    private readonly List<Action<IServiceCollection, Settings2>> _configureServicesActions = new();
    private readonly List<Action<Settings2>> _configureSettingsActions = new();


    public static CommandBuilder Create() => new();



    #region Configure

    public void Configure(Action<Settings2> configureSettingsDelegate)
    {
        _configureSettingsActions.Add(configureSettingsDelegate);
    }



    public void ConfigureCommands(Action<ICommandDescriptorCollection> configureCommandsAction)
    {
        _configureCommandsActions.Add(configureCommandsAction);
    }



    public void ConfigureCommandlineArguments(Action<IArgumentCollection, Settings2> action)
    {
        _configureArgumentsActions.Add(action);
    }



    public void ConfigureArgumentConverters(Action<Settings2> action)
    {
        _configureArgumentConvertersActions.Add(action);
    }



    public void ConfigureServices(Action<IServiceCollection, Settings2> action)
    {
        _configureServicesActions.Add(action);
    }

    #endregion



    #region Build

    public Commander2 Build()
    {
        var settings = createDefaultSettings();
        foreach (var build in _configureSettingsActions) build(settings);

        // private ICommandResolver _commandResolver = new AssemblyCommandResolver(Assembly.GetEntryAssembly());
        var commandDescriptors = createDefaultCommandDescriptors(settings);
        foreach (var build in _configureCommandsActions) build(commandDescriptors);

        IServiceCollection services = new ServiceCollection();
        services.AddCLArgsServices();
        services.AddScoped(_ => settings);
        services.AddScoped(_ => commandDescriptors);
        
        IArgumentCollection arguments = new ArgumentCollection();
        services.AddScoped(_ => arguments);

        foreach (var commandDescriptor in commandDescriptors.Values)
            services.AddScoped(commandDescriptor.Type);

        foreach (var action in _configureServicesActions)
            action(services, settings);
        
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        createDefaultArguments(serviceProvider, arguments);
        foreach (var build in _configureArgumentsActions) build(arguments, settings);


        return serviceProvider.GetRequiredService<Commander2>();
    }

    private void createDefaultArguments(IServiceProvider serviceProvider, IArgumentCollection arguments)
    {
        CommandLineParser2 cp = serviceProvider.GetRequiredService<CommandLineParser2>();
        cp.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray(), arguments);
    }



    private Settings2 createDefaultSettings() => new() { IgnoreCase = true };



    /// <summary>
    ///     Query the current CLArgs Assembly for [out-of-the-box] ICommand2 implementations.
    /// </summary>
    private ICommandDescriptorCollection createDefaultCommandDescriptors(Settings2 settings)
    {
        var result = new CommandDescriptorCollection(settings);
        result.AddAssembly(Assembly.GetEntryAssembly());
        return result;
    }

    #endregion
}