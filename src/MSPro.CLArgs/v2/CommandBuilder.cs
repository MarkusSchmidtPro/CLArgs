using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace MSPro.CLArgs;

[PublicAPI]
public class CommandBuilder
{
    private readonly List<Action<ICommandlineArgumentCollection, Settings2>> _configureCommandlineArgumentsActions =
        new();

    private readonly List<Action<ICommandDescriptorCollection>> _configureCommandsActions = new();
    private readonly List<Action<IServiceCollection, Settings2>> _configureServicesActions = new();
    private readonly List<Action<Settings2>> _configureSettingsActions = new();
    private readonly List<Action<Settings2>> _configureArgumentConvertersActions = new();


    public static CommandBuilder Create() => new();

    #region Configure

    public void Configure(Action<Settings2> configureSettingsDelegate)
    {
        _configureSettingsActions.Add(configureSettingsDelegate);
    }


    public void ConfigureCommands(Action<ICommandDescriptorCollection> configureCommandDescriptorsAction)
    {
        _configureCommandsActions.Add(configureCommandDescriptorsAction);
    }


    public void ConfigureCommandlineArguments(Action<ICommandlineArgumentCollection, Settings2> action)
    {
        _configureCommandlineArgumentsActions.Add(action);
    }

    public void ConfigureArgumentConverters(Action<Settings2> configureArgumentConvertersAction)
    {
        _configureArgumentConvertersActions.Add(configureArgumentConvertersAction);
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
        ICommandDescriptorCollection commandDescriptors = createDefaultCommandDescriptors();
        foreach (var build in _configureCommandsActions) build(commandDescriptors);

        ICommandlineArgumentCollection commandlineArguments = createDefaultCommandlineArguments();
        foreach (var build in _configureCommandlineArgumentsActions) build(commandlineArguments, settings);
     
        IArgumentConvertersCollection argumentConverters = createDefaultArgumentConverters();
        foreach (var build in _configureArgumentConvertersActions) build( settings);

        IServiceCollection services = new ServiceCollection();
        services.AddScoped<Settings2>();
        services.AddScoped<OptionResolver2>();
        services.AddScoped<ContextBuilder>();
        services.AddScoped(_ => commandDescriptors);
        services.AddScoped(_ => commandlineArguments);
        services.AddScoped(_ => argumentConverters);
        foreach (var action in _configureServicesActions) action(services, settings);

        foreach (var commandDescriptor in commandDescriptors.Values)
            services.AddScoped(commandDescriptor.Type);

        return new Commander2(services.BuildServiceProvider());
    }

    private IArgumentConvertersCollection createDefaultArgumentConverters()
    {
        var result = new ArgumentConvertersCollection
        {
            { typeof(string), new StringArgumentConverter() },
            { typeof(int), new IntArgumentConverter() },
            { typeof(bool), new BoolArgumentConverter() },
            { typeof(DateTime), new DateTimeArgumentConverter() },
            { typeof(Enum), new EnumArgumentConverter() }
        };
        return result;
    }


    private ICommandlineArgumentCollection createDefaultCommandlineArguments() => new CommandlineArgumentCollection();

    private Settings2 createDefaultSettings() => new();

    private ICommandDescriptorCollection createDefaultCommandDescriptors() => new CommandDescriptorCollection();

    #endregion
}