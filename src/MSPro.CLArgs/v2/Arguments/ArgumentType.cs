namespace MSPro.CLArgs
{
    /// <summary>
    /// The type of a command-argument.
    /// </summary>
    public enum ArgumentType
    {
        /// <summary>
        /// A Verb specifies an action.
        /// The provided command-line typically starts with one or more verb before the options.
        /// The list of verbs determine which <see cref="ICommand2">Command</see> is executed. 
        /// </summary>
        Verb,
    
        /// <summary>
        /// Option configure the behaviour of the current command.
        /// </summary>
        Option,
    
        /// <summary>
        /// Targets are some kind of special options,
        /// specified at the end of the command-line arguments.
        /// </summary>
        Target
    }
}