﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



namespace MSPro.CLArgs
{
    public class CommandHostBuilder : IHostBuilder //CommandBuilder
    {
        private readonly List<Action<IOptionValueConverterCollection, Settings2>> _configureArgumentConvertersActions = [];
        private readonly List<Action<IArgumentCollection, Settings2>> _configureArgumentsActions = [];
        private readonly List<Action<ICommandDescriptorCollection>> _configureCommandsActions = [];
        private readonly List<Action<IServiceCollection, Settings2>> _configureServicesActions = [];
        private readonly List<Action<Settings2>> _configureSettingsActions = [];

        private readonly IHostBuilder _hostBuilder;

     
        private CommandHostBuilder()
        {
            _hostBuilder = Host.CreateDefaultBuilder();
        }
    
    
        #region IHostBuilder
        IDictionary<object, object> IHostBuilder.Properties => _hostBuilder.Properties;



        IHostBuilder IHostBuilder.ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureHostConfiguration(configureDelegate);
        }



        IHostBuilder IHostBuilder.ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureAppConfiguration(configureDelegate);
        }



        IHostBuilder IHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return _hostBuilder.ConfigureServices(configureDelegate);
        }



        IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            return _hostBuilder.UseServiceProviderFactory(factory);
        }



        IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            return _hostBuilder.UseServiceProviderFactory(factory);
        }



        IHostBuilder IHostBuilder.ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureContainer(configureDelegate);
        }

        #endregion
   
        public static CommandHostBuilder Create(string[] args)
        {
            var builder = new CommandHostBuilder();
            builder.ConfigureDefaults(args);
            builder.ConfigureCommandlineArguments((arguments, settings2) =>
            {
                arguments.AddCommandLine(args, settings2);
            });
            return builder;
        }



        public IHost Build()
        {
            var settings = createDefaultSettings();
            foreach (var build in _configureSettingsActions) build(settings);

            var commandDescriptors = createDefaultCommandDescriptors(settings);
            foreach (var build in _configureCommandsActions) build(commandDescriptors);

            IArgumentCollection arguments = new ArgumentCollection();
            foreach (var build in _configureArgumentsActions) build(arguments, settings);

            _hostBuilder.ConfigureServices( services =>
            {
                services.AddCLArgsServices();
                services.AddScoped(_ => settings);
                services.AddScoped(_ => commandDescriptors);
                services.AddScoped(_ => arguments);

                foreach (var commandDescriptor in commandDescriptors.Values)
                {
                    services.AddScoped(commandDescriptor.Type);
                }

                foreach (var action in _configureServicesActions)
                {
                    action(services, settings);
                }

                // HostBuild returns the IHost instance
                services.AddScoped<IHost,CommandHost>();
            });

            return _hostBuilder.Build();
        }
    

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



        public void ConfigureArgumentConverters(Action<IOptionValueConverterCollection, Settings2> action)
        {
            _configureArgumentConvertersActions.Add(action);
        }

        #endregion



        #region Defaults


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
}