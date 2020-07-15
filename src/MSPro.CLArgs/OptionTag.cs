namespace MSPro.CLArgs
{
    /// <summary>
    ///     A command-line Option.
    /// </summary>
    public class OptionTag
    {
        /// <summary>
        ///     Create a new options.
        /// </summary>
        /// <param name="tag">The name of the option.</param>
        public OptionTag(string tag)
        {
            this.Tag = tag;
        }



        public string Tag { get; }
        public string Value { get; set; }

        public override string ToString() => this.Value;
    }
}