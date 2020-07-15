using System.Collections.Generic;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     The arguments as they were provided as a command-line.
    /// </summary>
    /// <remarks>
    ///     An <c>Argument</c> can be either a <see cref="Verbs" />
    ///     or an <see cref="OptionTag" />.
    /// </remarks>
    public class Arguments
    {
        public Arguments(string commandLine)
        {
            this.CommandLine = commandLine;
            this.Verbs       = new List<string>();
            this.Options     = new Dictionary<string, OptionTag>();
        }



        public string CommandLine { get; }


        /// <summary>
        ///     The list of verbs in the sequence order
        ///     as they were provided in the command-line.
        /// </summary>
        public List<string> Verbs { get; }


        public string VerbPath => string.Join(".", this.Verbs);

        /// <summary>
        ///     A key-value list of all options provided in the command-line.
        /// </summary>
        /// <remarks>
        ///     All option values are <c>strings</c> int he first instance.
        ///     Conversion may happen later.
        /// </remarks>
        public Dictionary<string, OptionTag> Options { get; }



        public void AddOption(OptionTag o)
        {
            this.Options[o.Tag] = o;
        }
    }
}