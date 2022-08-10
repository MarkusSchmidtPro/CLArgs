using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace MSPro.CLArgs;

[PublicAPI]
public class CommandBuilder
{
    private readonly List<Action<Settings2>> _configureArgumentConvertersActions = new();

    private readonly List<Action<IArgumentCollection, Settings2>> _configureArgumentsActions =
        new();

    private readonly List<Action<ICommandDescriptorCollection>> _configureCommandsActions = new();
    private readonly List<Action<IServiceCollection, Settings2>> _configureServicesActions = new();
    private readonly List<Action<Settings2>> _configureSettingsActions = new();


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


    public void ConfigureCommandlineArguments(Action<IArgumentCollection, Settings2> action)
    {
        _configureArgumentsActions.Add(action);
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
        var commandDescriptors = createDefaultCommandDescriptors();
        foreach (var build in _configureCommandsActions) build(commandDescriptors);

        var arguments = createDefaultArguments(settings);
        foreach (var build in _configureArgumentsActions) build(arguments, settings);

        var argumentConverters = createDefaultArgumentConverters();
        foreach (var build in _configureArgumentConvertersActions) build(settings);

        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<OptionResolver2>();
        services.AddSingleton<ContextBuilder>();
        services.AddSingleton(settings);
        services.AddScoped(_ => commandDescriptors);
        services.AddScoped(_ => arguments);
        services.AddScoped(_ => argumentConverters);

        foreach (var commandDescriptor in commandDescriptors.Values)
            services.AddScoped(commandDescriptor.Type);

        foreach (var action in _configureServicesActions)
            action(services, settings);

        return new Commander2(services.BuildServiceProvider());
    }

    private IArgumentConverterCollection createDefaultArgumentConverters()
    {
        var result = new ArgumentConverterCollection
        {
            { typeof(string), new StringArgumentConverter() },
            { typeof(int), new IntArgumentConverter() },
            { typeof(bool), new BoolArgumentConverter() },
            { typeof(DateTime), new DateTimeArgumentConverter() },
            { typeof(Enum), new EnumArgumentConverter() }
        };
        return result;
    }


    private IArgumentCollection createDefaultArguments(Settings2 settings)
    {
        var result = new ArgumentCollection();
        result.AddArguments(Environment.GetCommandLineArgs(), settings);
        return result;
    }


    private Settings2 createDefaultSettings() => new();

    private ICommandDescriptorCollection createDefaultCommandDescriptors()
    {
        var result = new CommandDescriptorCollection();
        result.AddAssemblies(new AssemblyCommandResolver2(Assembly.GetExecutingAssembly()));
        return result;
    }

    #endregion
}