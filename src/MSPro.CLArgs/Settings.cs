using System;
using System.Reflection;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [PublicAPI]
    public class Settings
    {
        internal void Trace(string message) => TraceMessage?.Invoke(message);
        public Action<string> TraceMessage { get; set; } = null;
        
        public bool IgnoreCase { get; set; } = false;


        /// <summary>
        /// Automatically resolve commands using <see cref="CommandResolver"/>
        /// </summary>
        public bool AutoResolveCommands { get; set; } = false;
        
        /// <summary>
        ///     Get or set an object to resolve all known commands (and verbs).
        /// </summary>
        public ICommandResolver CommandResolver { get; set; } = new AssemblyCommandResolver(Assembly.GetEntryAssembly());
        
        
        /// <summary>
        ///     Get or set a list of characters that mark the end of an option's name.
        /// </summary>
        public char[] OptionValueTags = {' ', ':', '='};

        /// <summary>
        ///     Get or set tags which identify an option.
        /// </summary>
        /// <remarks>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.
        /// </remarks>
        public char[] OptionsTags { get; set; } = {'-', '/'};
    }
}