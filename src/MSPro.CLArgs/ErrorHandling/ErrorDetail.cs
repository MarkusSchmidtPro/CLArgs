using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable enable



namespace MSPro.CLArgs
{
    [DebuggerDisplay("{ErrorMessages[0]}")]
    public class ErrorDetail(string attributeName, IEnumerable<string>? errors = null, string? stackTrace = null)
    {
        public string? StackTrace { get; set; } = stackTrace;

        //[JsonProperty("attributeName", Order = 10, Required = Required.Always, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string AttributeName { get; set; } = attributeName;


        //[JsonProperty("errorMessages", Order = 20, Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> ErrorMessages { get; set; } = errors != null ? errors.ToList() : [];
    }
}