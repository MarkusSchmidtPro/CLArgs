using System.Collections.Generic;
using MSPro.CLArgs;



namespace CLArgs.Command.CommandCollection.SumCommand
{
    internal class SumParameters
    {
        [OptionDescriptor("Value", "v", 
            Default = 0, Required = true, 
            AllowMultiple = nameof(Values), 
            AllowMultipleSplit = ";")]
        public string Value { get; set; }

        public List<string> Values { get; set; } = new();
    }
}