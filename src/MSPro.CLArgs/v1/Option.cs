﻿using System.Diagnostics;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     A command-line Option.
    /// </summary>
    [DebuggerDisplay( "{Key}='{Value}', resolved={IsResolved}")]
    public class Option
    {
        /// <summary>
        ///     Create a new options.
        /// </summary>
        /// <param name="tag">The name of the option.</param>
        /// <param name="value"></param>
        public Option(string tag, string value) : this(tag) => Value = value;



        /// <summary>
        ///     Create an unresolved Option.
        /// </summary>
        /// <remarks>
        ///     Unresolved means no value has been set.
        /// </remarks>
        public Option(string tag) => Key = tag;



        /// <summary>
        ///     Get the option's Key - which is the tag as it was provided in the command-line,
        ///     or the option's name, after option resolution.
        /// </summary>
        public string Key { get; }

        public string Value { get; set; }

        public bool IsResolved => Value != null;
    }
}