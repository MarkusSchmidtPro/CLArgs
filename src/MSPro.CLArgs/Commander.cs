using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    
    /// <summary>
    /// The top level class to easily use 'CLArgs'.
    /// </summary>
    [PublicAPI]
    public class Commander
    {
        private readonly Dictionary<string, Func<ICommand>> _commands ;
        private readonly Settings _settings;


        /// <summary>
        ///     Create a new Commander instance.
        /// </summary>
        /// <param name="args">The arguments as provided to <code>void Main( string[] args)</code>.</param>
        /// <param name="settings">Settings used to control CLArgs overall behaviour.</param>
        /// <remarks>
        ///     When creating an instance the provided <see cref="args" /> will
        ///     be parsed and Command implementations will be resolved (in case <see cref="Settings.AutoResolveCommands" /> is set
        ///     to true.
        /// </remarks>
        public Commander([NotNull] string[] args, Settings settings=null)
        {
            _settings = settings ?? new Settings();
            _commands = new Dictionary<string, Func<ICommand>>();
                
            this.Arguments1 = CommandLineParser.Parse(args);
            // Resolve commands and register CommandBase factories
            if (_settings.AutoResolveCommands) resolveCommandImplementations();
        }



        /// <summary>
        ///     The Verbs and Options as they were provided in the command-line.
        /// </summary>
        public Arguments Arguments1 { get; }



        /// <summary>
        ///     Manually register (add or update) a Command.
        /// </summary>
        /// <remarks>
        ///     Not the command itself is registered but <b>a factory function</b> that
        ///     is used to create a new instance of the Command.<br />
        ///     If there is already a command registered for the same <paramref name="verb" />
        ///     the 'old' command is overridden.
        /// </remarks>
        /// <param name="verb">The <see cref="Arguments.Verbs" /> that is linked to this Command</param>
        /// <param name="factory">A factory function that return an instance of <see cref="CommandBase{TCommandParameters}" />.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="verb" /> is null or empty.</exception>
        /// <example>
        ///     <code>
        ///     Commander c = new Commander(args);
        ///     c.RegisterCommandFactory( "HelloWorld", () => new HelloWorldCommand());
        ///     c.ExecuteCommand();
        /// </code>
        /// </example>
        /// <seealso cref="Settings.AutoResolveCommands" />
        public void RegisterCommandFactory([NotNull] string verb, [NotNull] Func<ICommand> factory)
        {
            if (string.IsNullOrEmpty(verb)) throw new ArgumentNullException(nameof(verb));
            _commands[verb] = factory;
        }



        /// <summary>
        ///     Resolve a Command implementation by Verb.
        /// </summary>
        /// <seealso cref="Settings.AutoResolveCommands" />
        /// <seealso cref="RegisterCommandFactory" />
        /// <param name="verb">The verb for which and implementation should be resolved.</param>
        public ICommand ResolveCommand([NotNull] string verb)
        {
            if (string.IsNullOrEmpty(verb)) throw new ArgumentNullException(nameof(verb));
            if (_commands==null || _commands.Count == 0) 
                throw new ApplicationException("No Commands have been registered");
            if (!_commands.ContainsKey(verb))
                throw new IndexOutOfRangeException(
                    $"There is no Command registered for verb ${verb}. "
                    + "Check if upper/lower case is correct.");

            return _commands[verb](); // call construction method
        }




        /// <summary>
        ///     Execute the command referenced by <see cref="Arguments.VerbPath" />.
        /// </summary>
        public void ExecuteCommand()
        {
            if (_commands==null || _commands.Count == 0)
                throw new ApplicationException("No Commands have been registered");
            
            ICommand command = ResolveCommand(this.Arguments1.VerbPath);
            command.Execute(this.Arguments1, _settings);
        }



        /// <summary>
        ///     Shortcut and preferred way to use Commander.
        /// </summary>
        public static void ExecuteCommand(string[] args, Settings settings=null) => new Commander(args, settings).ExecuteCommand();



        private void resolveCommandImplementations()
        {
            Dictionary<string, Type> verbAndCommandTypes = this._settings.CommandResolver.GetCommandTypes();
            if (verbAndCommandTypes.Count == 0)
                throw new ApplicationException(
                    $"{nameof(this._settings.AutoResolveCommands)} is {this._settings.AutoResolveCommands} " +
                    $"however the resolver {this._settings.CommandResolver.GetType()} did not find any CommandBase implementation! " +
                    "Make sure the resolver can see/find the Commands.");

            foreach (var commandType in verbAndCommandTypes)
            {
                RegisterCommandFactory(commandType.Key,
                                       () => (ICommand) Activator.CreateInstance(commandType.Value));
            }
        }
    }
}