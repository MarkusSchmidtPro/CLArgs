namespace MSPro.CLArgs
{
    public interface IHelpBuilder
    {
        /// <summary>
        ///     Build the help text that is displayed as general help text.
        /// </summary>
        public string BuildAllCommandsHelp();



        /// <summary>
        ///     Display a formatted help text for a single Command.
        /// </summary>
        /// <remarks>
        ///     This method is called when a VERB was specified and help for this particular verb should be printed.<br />
        ///     The <see cref="CommandDescriptor">Command's descriptor</see> is passed to the method (the Command is specified by
        ///     the Verb).
        /// </remarks>
        public string BuildCommandHelp(CommandDescriptor2 c);
    }
}