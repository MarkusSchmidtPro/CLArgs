using System;
using System.Collections.Generic;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs
{
    public interface  ICommand
    {
        void Execute(Arguments commandParameters, Settings settings);
        
        /*/// <summary>
        ///     The handler that is called when the command execution completed.
        /// </summary>
        /// <remarks>
        ///     The default handler does nothing on success or it throws an
        ///     <see cref="AggregateException" /> in case <see cref="Errors" /> contains any record.
        ///     Depending on the <see cref="Settings.TraceLevel" /> errors might be traced before.
        /// </remarks>
        Action<ErrorDetailList> OnCompleted { get; set; }


        public void OnResolvePropertiesHandler (
            object commandParameters,
            HashSet<string> unresolvedPropertyNames){}*/
    }
}