using MSPro.CLArgs;



namespace CLArgs.Command.CommandCollection.JsonToDBCommand
{
    internal class JsonToDbParameters
    {
        [OptionSet]
        public UsernamePassword UsernamePassword { get; set; }
        
        [OptionDescriptor("Filename", "f", Required = true)]
        public string Filename { get; set; }
        
        [OptionDescriptor("MaxItems", "m", Default = 10)]
        public int MaxItems { get; set; }
    }
}