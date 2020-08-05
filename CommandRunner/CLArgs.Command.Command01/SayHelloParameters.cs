using MSPro.CLArgs;



namespace CLArgs.Command.Command01
{
    public class SayHelloParameters
    {
        [OptionDescriptor("Country",  Required = true)]
        public string Country { get; set; }

        [OptionDescriptor( "Count", Required = false, Default = 1)]
        public int Count { get; set; }
    }
}