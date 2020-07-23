using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [PublicAPI]
    public class Commander
    {
        private readonly Dictionary<string, Func<ICommand>> _commands = new Dictionary<string, Func<ICommand>>();


        public static Settings Settings { get; private set; } = new Settings();

        
        public Arguments Arguments { get; }

        public void RegisterCommandFactory(Func<ICommand> factory) => RegisterCommandFactory("", factory);



        public void RegisterCommandFactory(string verb, Func<ICommand> factory)
        {
            if (_commands.Count > 0 && _commands.First().Key == "")
                throw new ApplicationException(
                    "You cannot register mor than one command if you registered a Default command.");
            _commands.Add(verb, factory);
        }



        public ICommand GetInstance(string verb = null) =>
            string.IsNullOrWhiteSpace(verb) && _commands.ContainsKey(verb) ? _commands[verb]() : GetDefault();



        public ICommand GetDefault() => _commands.Count == 0 ? null : _commands.First().Value();



        /// <summary>
        ///     Execute the command referenced by <see cref="Arguments.VerbPath" />.
        /// </summary>
        public void ExecuteCommand()
        {
            ICommand command = GetInstance(this.Arguments.VerbPath);
            command.Execute(this.Arguments);
        }



        #region Construction

        /// <summary>
        ///     Parse the command-line into arguments.
        /// </summary>
        /// <remarks>
        ///     You may modify the <see cref="Arguments" /> as needed (add/remove/change)
        ///     and then continue with <see cref="Commander( Arguments, Settings)" />
        /// </remarks>
        public static Arguments ParseCommandLine(string[] args) => new Parser().Run(args);



        public Commander([NotNull] string[] args) : this(ParseCommandLine(args))
        {
        }



        /// <summary>
        ///     Create a new Commander instance.
        /// </summary>
        /// <param name="argument">The parsed arguments.</param>
        /// <param name="settings">Provide your own settings to adjust the Commanders behaviour.</param>
        public Commander([NotNull] Arguments arguments)
        {
            this.Arguments = arguments;

            // Resolve commands and register ICommand factories
            if (Settings.AutoResolveCommands) resolveCommandImplementations();
            
        }



        private void resolveCommandImplementations()
        {
            Dictionary<string, Type> verbAndCommandTypes = Settings.CommandResolver.GetCommandTypes();
            if (verbAndCommandTypes.Count == 0)
                throw new ApplicationException(
                    $"{nameof(Settings.AutoResolveCommands)} is {Settings.AutoResolveCommands} " +
                    $"however the resolver {Settings.CommandResolver.GetType()} did not find any ICommand implementation! " +
                    "Make sure the resolver can see/find the Commands.");

            foreach (var commandType in verbAndCommandTypes)
            {
                RegisterCommandFactory(commandType.Key,
                                       () => (ICommand) Activator.CreateInstance(commandType.Value));
            }
        }

        #endregion
    }
}