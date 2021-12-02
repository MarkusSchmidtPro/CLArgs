using MSPro.CLArgs;



namespace CLArgs.Command.CommandCollection.AddCommand
{
    internal class AddParameters
    {
        [OptionDescriptor("Value1", "v1", Default = 0, Required = true)]
        public int Value1 { get; set; }

        [OptionDescriptor("Value2", "v2", Default = 0, Required = true)]
        public int Value2 { get; set; }

        [OptionDescriptor("Value3", "v3", Default = 0, Required = false)]
        public int Value3 { get; set; }
    }
}