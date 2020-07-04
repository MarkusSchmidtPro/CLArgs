using System;
using System.Diagnostics;



namespace MSPro.CLArgs.Contract
{
    /// <summary>
    ///     Application execution properties.
    /// </summary>
    public class AppExecutionProperties
    {
        private static AppExecutionProperties _instance;



        private AppExecutionProperties()
        {
            int d = (DateTime.UtcNow - new DateTime(2020, 2, 4)).Days;
            int s = (int) DateTime.UtcNow.TimeOfDay.TotalSeconds;
            this.ExecutionId = $"{d:0000}#{s:00000}";
            this.ProcessId = Process.GetCurrentProcess().Id;
        }



        /// <summary>
        ///     The Id of the current console runner process execution.
        /// </summary>
        /// <remarks>
        ///     It is vital for the application's behavior that this number is sequential (=sortable) - not random / not Guid.
        ///     Default implementation is $"{NoOfDaysSince_2020-04-02:0000}#{TotalSecondsToday:00000}"
        /// </remarks>
        public string ExecutionId { get; }

        public int ProcessId { get; }
        public DateTime StartDateUtc { get; set; }
        public static AppExecutionProperties Get()
        {
            if( _instance == null) _instance = new AppExecutionProperties();
            return _instance;
        }
    }
}