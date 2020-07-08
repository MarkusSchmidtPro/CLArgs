using System.Collections.Generic;

namespace MSPro.CLArgs
{
    /// <summary>
    ///     The arguments as they were provided as a command-line.
    /// </summary>
    /// <remarks>
    ///     An <c>Argument</c> can be either a <see cref="Verbs" />
    ///     or an <see cref="Option" />.
    /// </remarks>
    public class Arguments
    {
        public Arguments(string commandLine)
        {
            CommandLine = commandLine;
            Verbs = new List<string>();
            Options = new Dictionary<string, Option>();
        }

        public string CommandLine { get; }


        /// <summary>
        ///     The list of verbs in the sequence order
        ///     as they were provided in the command-line.
        /// </summary>
        public List<string> Verbs { get; }

        /// <summary>
        ///     A key-value list of all options provided in the command-line.
        /// </summary>
        /// <remarks>
        ///     All option values are <c>strings</c> int he first instance.
        ///     Conversion may happen later.
        /// </remarks>
        public Dictionary<string, Option> Options { get; }


        public void AddOption(Option o)
        {
            Options[o.Name] = o;
        }
    }
}