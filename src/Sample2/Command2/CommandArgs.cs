namespace MSPro.CLArgs.Sample2.Command2
{
    class CommandArgs
    {
        [CommandLineOption("Option 1",  new[] {"--opt1", "-o"}, Mandatory=true)]
        public bool Option1 { get; set; }

        [CommandLineOption("Option 2",  "--opt2", Mandatory=true)]
        public int Option2 { get; set; }

        [CommandLineOption("opt3")]
        public string Option3 { get; set; }
    }
}