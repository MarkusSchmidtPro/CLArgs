using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;



namespace MSPro.CLArgs;

[DebuggerDisplay("{ErrorMessages[0]}")]
public class ErrorDetail
{
    public ErrorDetail(string attributeName, IEnumerable<string> errors = null)
    {
        this.AttributeName = attributeName;
        this.ErrorMessages = errors != null ? errors.ToList() : new List<string>();
    }



    //[JsonProperty("attributeName", Order = 10, Required = Required.Always, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
    public string AttributeName { get; set; }


    //[JsonProperty("errorMessages", Order = 20, Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<string> ErrorMessages { get; set; }
}