﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     The top level class to easily use 'CLArgs'.
    /// </summary>
    public class Commander
    {
        // Internally use a dictionary to make sure Verbs are unique
        private readonly Dictionary<string, CommandDescriptor> _commandDescriptors;
        private readonly Settings _settings;



        /// <summary>
        ///     Create a new Commander instance.
        /// </summary>
        /// <param name="settings">Settings used to control CLArgs overall behaviour.</param>
        public Commander(Settings settings = null)
        {
            _settings           = settings ?? new Settings();
            _commandDescriptors = new Dictionary<string, CommandDescriptor>(_settings.GetStringComparer());
            // Resolve commands and register ICommand factories
            if (_settings.AutoResolveCommands) resolveCommandImplementations();
        }



        /// <summary>
        ///     Get a list of available CommandDescriptors.
        /// </summary>
        public List<CommandDescriptor> CommandDescriptors => _commandDescriptors.Values.ToList();



        /// <summary>
        ///     Manually register (add or update) a Command.
        /// </summary>
        /// <remarks>
        ///     Not the command itself is registered but <b>a factory function</b> that
        ///     is used to create a new instance of the Command.<br />
        ///     If there is already a command registered for the same <paramref name="verb" />
        ///     the 'old' command is overridden.
        /// </remarks>
        /// <param name="verb">The <see cref="CommandLineArguments.Verbs" /> that is linked to this Command</param>
        /// <param name="factoryFunc">A factory function that return an instance of <see cref="ICommand" />.</param>
        /// <param name="commandDescription"></param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="verb" /> is null or empty.</exception>
        /// <example>
        ///     <code>
        ///     Commander c = new Commander(args);
        ///     c.RegisterCommandFactory( "HelloWorld", () => new HelloWorldCommand());
        ///     c.ExecuteCommand();
        /// </code>
        /// </example>
        /// <seealso cref="Settings.AutoResolveCommands" />
        [Obsolete("Use RegisterCommand() instead.")]
        public void RegisterCommandFactory([NotNull] string verb, [NotNull] Func<ICommand> factoryFunc,
            string commandDescription = null)
            => RegisterCommand(new CommandDescriptor(verb, factoryFunc, commandDescription));



        /// <summary>
        ///     Register a command.
        /// </summary>
        /// <param name="commandDescriptor"></param>
        public void RegisterCommand(CommandDescriptor commandDescriptor)
            => _commandDescriptors[commandDescriptor.Verb] = commandDescriptor;



        /// <summary>
        ///     Directly bind a function to a verb.
        /// </summary>
        /// <param name="verb">The Verb</param>
        /// <param name="func">
        ///     The function that is executed when the verbs passed in the
        ///     command-line (<see cref="CommandLineArguments.VerbPath" /> are equal to the <paramref name="verb" />.
        /// </param>
        /// <param name="commandDescription"></param>
        /// <example>
        ///     <code>
        /// string COMMAND_LINE = "word1 text2 verb3";
        /// // Arguments.VerbPath is executed
        /// var commander = new Commander(new Settings { AutoResolveCommands = false });
        /// commander.RegisterFunction("word1", word);
        /// commander.RegisterFunction("word1.text2", text);
        /// commander.RegisterFunction("word1.text2.verb3", verb);
        /// commander.ExecuteCommand(args);
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">In case <paramref name="verb" /> is null.</exception>
        /// <seealso cref="CommandLineArguments.VerbPath " />
        /// <seealso cref="Settings.AutoResolveCommands" />
        public void RegisterFunction([NotNull] string verb, [NotNull] Action<CommandLineArguments> func,
            string commandDescription = null)
        {
            if (string.IsNullOrEmpty(verb)) throw new ArgumentNullException(nameof(verb));
            _commandDescriptors[verb] = new CommandDescriptor(verb, () => new CommandWrapper(func), commandDescription);
        }



        /// <summary>
        ///     Resolve a Command implementation by Verb.
        /// </summary>
        /// <seealso cref="Settings.AutoResolveCommands" />
        /// <seealso cref="RegisterCommandFactory" />
        /// <param name="verb">
        ///     The verb for which and implementation should be resolved.
        ///     IF <c>verb</c> is <c>null</c> the default Command (first registered command) is returned.
        /// </param>
        /// <param name="throwOnFail">
        ///     If <c>true</c> the method throws an exception in case
        ///     "There is no Command registered for verb..". If set to <c>false</c> it returns <c>null</c>.
        /// </param>
        public CommandDescriptor ResolveCommand(string verb, bool throwOnFail = true)
        {
            if (_commandDescriptors == null || _commandDescriptors.Count == 0)
                throw new ApplicationException("No Commands have been registered.");

            if (string.IsNullOrEmpty(verb)) 
                throw new ArgumentNullException(nameof(verb),"Cannot resolve a command if no verb is specified.");

            if (_commandDescriptors.ContainsKey(verb)) return _commandDescriptors[verb];

            if (throwOnFail)
                throw new IndexOutOfRangeException(
                    $"There is no Command registered for verb '{verb}'. "
                    + "Check if upper/lower case is correct.");
            return null;
        }



        /// <summary>
        ///     Execute the command referenced by <see cref="CommandLineArguments.VerbPath" />.
        /// </summary>
        public void ExecuteCommand([NotNull] CommandLineArguments commandLineArguments)
        {
            if (_commandDescriptors == null || _commandDescriptors.Count == 0)
                throw new ApplicationException("No Commands have been registered");

            if (!commandLineArguments.Verbs.Any())
            {
                if(  !commandLineArguments.Targets.Any() &&
                     (commandLineArguments.Options.Count==0 || commandLineArguments.HelpRequested))
                {
                    _settings.DisplayAllCommandsDescription?.Invoke(CommandDescriptors);
                    return;
                }
                
                // No explicit Verb specified in command-line
                // set default verb
                commandLineArguments.AddVerb("DEFAULT");
            }
            
            
            var commandDescriptor = ResolveCommand(commandLineArguments.VerbPath, true);

            if (commandDescriptor == null
                && commandLineArguments.Verbs.Count > 0
                && commandLineArguments.Verbs[0].StartsWith("clargs", _settings.StringComparison))
            {
                commandDescriptor = ResolveCommand(commandLineArguments.Verbs[0]);
            }

            if (commandLineArguments.HelpRequested)
            {
                _settings?.DisplayCommandHelp(commandDescriptor);
                return;
            }
         
            commandDescriptor.CreateCommandInstance().Execute(commandLineArguments, _settings);
        }



        /// <summary>
        ///     Shortcut and preferred way to use Commander.
        /// </summary>
        /// <example>
        ///     Full code:
        ///     <code>
        ///     new Commander(settings).ExecuteCommand(CommandLineParser.Parse(args))
        ///     </code>
        /// </example>
        public static void ExecuteCommand(string[] args, Settings settings = null) =>
            new Commander(settings).ExecuteCommand(CommandLineParser.Parse(args, settings));



        private void resolveCommandImplementations()
        {
            List<CommandDescriptor> commandDescriptors = _settings.CommandResolver.GetCommandDescriptors();
            if (!commandDescriptors.Any())
                throw new ApplicationException(
                    $"{nameof(_settings.AutoResolveCommands)} is {_settings.AutoResolveCommands} " +
                    $"however the resolver {_settings.CommandResolver.GetType()} did not find any ICommand implementation! " +
                    "Make sure the resolver can see/find the Commands.");

            foreach (var commandDescriptor in commandDescriptors)
            {
                RegisterCommand(commandDescriptor);
            }
        }
    }
}