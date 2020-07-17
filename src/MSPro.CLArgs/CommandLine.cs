using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    [PublicAPI]
    public static class CommandLine
    {
        /// <summary>
        ///     Get or set a list of characters that mark the end of an option's name.
        /// </summary>
        public static char[] OptionValueTags = {' ', ':', '='};

        /// <summary>
        ///     Get or set tags which identify an option.
        /// </summary>
        /// <remarks>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.
        /// </remarks>
        /// <seealso cref="OptionTag" />
        public static char[] OptionsTags { get; set; } = {'-', '/'};



        /// <summary>
        ///     Parse the command-line arguments into a key-value list.
        /// </summary>
        public static Arguments Parse(string[] args)
            => new Parser(OptionsTags, OptionValueTags)
               .Run(string.Join(" ", args));
    }
}