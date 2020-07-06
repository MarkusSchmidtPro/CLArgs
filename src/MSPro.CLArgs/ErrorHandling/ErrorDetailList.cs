using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs.ErrorHandling
{
    public class ErrorDetailList
    {
        public List<ErrorDetail> Details { get; } = new List<ErrorDetail>();

        public bool HasErrors() => this.Details.Count > 0;


        public void AddError(string attributeName, string errorMessage, string recordId = null)
        {
            string key = recordId == null ? attributeName : $"{attributeName}#{recordId}";
            var err = this.Details.FirstOrDefault(d => d.AttributeName.Equals(key, StringComparison.InvariantCultureIgnoreCase) );
            if (err==null)
            {
                err = new ErrorDetail(attributeName, recordId);
                this.Details.Add( err);
            }

            // prevent duplicate messages for the same item
            if (err.ErrorMessages.Contains(errorMessage)) return;
            err.ErrorMessages.Add(errorMessage);
        }
    }
}