namespace MSPro.CLArgs
{
    /// <summary>
    ///     A command-line Option.
    /// </summary>
    public class Option
    {
        /// <summary>
        ///     Create a new options.
        /// </summary>
        /// <param name="name">The name of the option.</param>
        /// <param name="value">
        ///     The value of the option.<br />
        ///     Default: <string> 'True'</string>
        /// </param>
        public Option(string name, string value = "True")
        {
            this.Name = name;
            this.Value = value;
        }



        public string Name { get; }
        public string Value { get; set;  }
    }
}