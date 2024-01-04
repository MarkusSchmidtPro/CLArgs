using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable

namespace MSPro.CLArgs;

public class ErrorDetailList
{
    public List<ErrorDetail> Details { get; } = [];


    public bool HasErrors() => Details.Count > 0;



    public void AddException(Exception ex)
    {
        AddError("General Exception", 
            new[] { ex.Message, ex.InnerException != null ? ex.InnerException.Message : string.Empty }, 
            ex.StackTrace);
    }



    public void AddError(string attributeName, string errorMessage)
    {
        AddError(attributeName, new[] { errorMessage });
    }



    public void AddError(string attributeName, IEnumerable<string> errorMessages, string? stackTrace = null)
    {
        ErrorDetail attributeErr = Details.FirstOrDefault(
            d => d.AttributeName.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase));
        
        if (attributeErr == null)
        {
            // Create new entry for that attribute
            attributeErr = new ErrorDetail(attributeName)
            {
                StackTrace = stackTrace
            };
            Details.Add(attributeErr);
        }

        foreach (string errorMessage in errorMessages)
        {
            // prevent duplicate messages for the same item
            if (attributeErr.ErrorMessages.Contains(errorMessage)) return;
            attributeErr.ErrorMessages.Add(errorMessage);
        }
    }



    public void Add(ErrorDetailList childList)
    {
        foreach (ErrorDetail childListDetail in childList.Details)
        {
            AddError(childListDetail.AttributeName, childListDetail.ErrorMessages);
        }
    }



    /// <summary>
    ///     Easy way to get one complete message for all errors
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder msg = new($"{Details.Count} ERROR(s) occurred.\n");
        foreach (ErrorDetail detail in Details)
        {
            msg.AppendLine($"ERROR on {detail.AttributeName}");
            foreach (string errorMessage in detail.ErrorMessages)
            {
                msg.AppendLine($"\t{errorMessage}");
            }
        }

        return msg.ToString();
    }
}