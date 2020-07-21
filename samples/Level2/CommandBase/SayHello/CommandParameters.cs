using MSPro.CLArgs;



namespace Level2.CommandBaseSample.SayHello
{
    public class CommandParameters
    {
        [OptionDescriptor("Country",  Required = true)]
        public string Country { get; set; }

        [OptionDescriptor( "Count", Required = false, Default = 1)]
        public int Count { get; set; }
    }
}