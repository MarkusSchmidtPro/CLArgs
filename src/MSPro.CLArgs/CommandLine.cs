namespace MSPro.CLArgs
{
    public static class CommandLine
    {
        /// <summary>
        ///     Get or set a list of characters that mark the end of an option's name,
        ///     and start the option's value.
        /// </summary>
        public static char[] OptionValueTags = {' ', ':', '='};

        /// <summary>
        ///     Get or set tags which identify an option.
        /// </summary>
        /// <remarks>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.<br />
        ///     Default: '-', '/'
        /// </remarks>
        /// <seealso cref="OptionTag" />
        public static char[] OptionsTags { get; set; } = {'-', '/'};



        /// <summary>
        ///     Parse the command-line arguments into a key-value list.
        /// </summary>
        /// <returns>
        ///     Command-line arguments as key-value list.
        ///     Key is the option's name and value is the provided value as a string.<br />
        /// </returns>
        public static Arguments Parse(string[] args)
            => new Parser(OptionsTags, OptionValueTags)
                .Run(string.Join(" ", args));
    }
}