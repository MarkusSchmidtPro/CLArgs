using System;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public interface ICommand
    {
        Arguments Arguments{ get; }
        ErrorDetailList ValidationErrors { get; }


        void Execute();


        /// <summary>
        ///     Validate command-line arguments
        /// </summary>
        /// <param name="throwIf">
        ///     If set to <c>true</c> an <see cref="AggregateException" /> is thrown
        ///     in case of validation errors. Otherwise, in case of result=<c>false</c>,
        ///     
        /// </param>
        /// <returns>
        /// Returns <c>true></c> in case all arguments have been parsed without failure and and <see cref="Execute"/> is supposed to run.<br/>
        /// Returns <c>false</c> if <param name="throwIf">is <c>false</c></param> and if validation errors have been recognized.
        /// In that case you can check <see cref="ValidationErrors"/> for all validation errors that occured.
        /// </returns>
        bool ValidateAndParseArguments(bool throwIf = true);

    }
}