using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs.ErrorHandling
{
    [PublicAPI]
    public class ErrorDetailList
    {
        public List<ErrorDetail> Details { get; } = new List<ErrorDetail>();

        public bool HasErrors() => this.Details.Count > 0;



        public void AddError(string attributeName, string errorMessage)
            => AddError(attributeName, new[] {errorMessage});



        public void AddError(string attributeName, IEnumerable<string> errorMessages)
        {
            var err = this.Details.FirstOrDefault(
                d => d.AttributeName.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase));
            if (err == null)
            {
                err = new ErrorDetail(attributeName);
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
                AddError(childListDetail.AttributeName, childListDetail.ErrorMessages);
            }
        }
    }
}