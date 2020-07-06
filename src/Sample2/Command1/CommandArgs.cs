namespace MSPro.CLArgs.Sample2.Functionality1
{
    class CommandArgs
    {
        //[CLOption(Tags={"--opt1", "-o"}, Description="", Default="false", Mandatory=true)]
        public bool Option1 { get; set; }
        public int Option2 { get; set; }
        public string Option3 { get; set; }
    }
}