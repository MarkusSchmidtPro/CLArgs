using System;
using System.Collections.Generic;
using System.Linq;
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
        var commandDescriptors = createDefaultCommandDescriptors(settings);
        foreach (var build in _configureCommandsActions) build(commandDescriptors);

        var argumentConverters = createDefaultArgumentConverters();
        foreach (var build in _configureArgumentConvertersActions) build(settings);

        IArgumentCollection arguments = new ArgumentCollection();
        
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<OptionResolver2>();
        services.AddSingleton<ContextBuilder>();
        services.AddSingleton<CommandLineParser2>();
        services.AddSingleton(settings);
        services.AddSingleton(arguments);
        services.AddScoped(_ => commandDescriptors);
        services.AddScoped(_ => argumentConverters);

        foreach (var commandDescriptor in commandDescriptors.Values)
            services.AddScoped(commandDescriptor.Type);

        foreach (var action in _configureServicesActions)
            action(services, settings);

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        CommandLineParser2 cp = serviceProvider.GetRequiredService<CommandLineParser2>();
        cp.Parse(Environment.GetCommandLineArgs().Skip(1).ToArray(), arguments);
        
        foreach (var build in _configureArgumentsActions) build(arguments, settings);


        return new Commander2(serviceProvider);
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

    private Settings2 createDefaultSettings() => new () { IgnoreCase = true};

    private ICommandDescriptorCollection createDefaultCommandDescriptors(Settings2 settings)
    {
        var result = new CommandDescriptorCollection(settings);
        result.AddAssemblies(new AssemblyCommandResolver2(Assembly.GetExecutingAssembly()));
        return result;
    }

    #endregion
}