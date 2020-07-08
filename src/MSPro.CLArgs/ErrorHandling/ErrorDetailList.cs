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
            => AddError(attributeName, new[] {errorMessage}, recordId);



        public void AddError(string attributeName, IEnumerable<string> errorMessages, string recordId = null)
        {
            string key = recordId == null ? attributeName : $"{attributeName}#{recordId}";
            var err = this.Details.FirstOrDefault(d => d.AttributeName.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            if (err == null)
            {
                err = new ErrorDetail(attributeName, recordId);
                this.Details.Add(err);
            }

            foreach (string errorMessage in errorMessages)
            {
                // prevent duplicate messages for the same item
                if (err.ErrorMessages.Contains(errorMessage)) return;
                err.ErrorMessages.Add(errorMessage);
            }
        }



        public void Add(ErrorDetailList childList)
        {
            foreach (ErrorDetail childListDetail in childList.Details)
            {
                AddError(childListDetail.AttributeName, childListDetail.ErrorMessages, childListDetail.RecordId);
            }
        }
    }
}