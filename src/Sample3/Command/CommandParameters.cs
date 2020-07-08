namespace MSPro.CLArgs.Sample3.Command
{
    internal class CommandParameters
    {
        [CommandLineOption("Option 1", new[] {"opt1", "o"}, Mandatory = true)]
        public bool Option1 { get; set; }

        [CommandLineOption("Option 2", "opt2", Mandatory = true)]
        public int Option2 { get; set; }

        [CommandLineOption("opt3")] 
        public string Option3 { get; set; }

        [CommandLineOption("opt4", "opt4", Default = "abc123")]
        public string Option4 { get; set; }


        // not provided via command-line, no default
        //[CommandLineOption("opt5", "opt5")]
        //public string Option5 { get; set; }
    }
}