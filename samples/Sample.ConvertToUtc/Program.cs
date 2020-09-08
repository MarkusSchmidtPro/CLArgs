using System;
using System.Linq;
using MSPro.CLArgs;



namespace CLArgs.Sample.ConvertToUtc
{
    /// <summary>
    ///     This application converts a given datetime (incl. time-zone) into UTC.
    ///     The application does not supports *Verbs* or *Targets*, it simply uses *Options*.
    /// </summary>
    internal static class Program
    {
        
        /// <summary>
        /// The test command-line for this example.
        /// </summary>
        private const string COMMAND_LINE = "--LocalDateTime='2020-08-01 08:10:00' --LocalTimeZone='Pacific Standard Time'";


        /// <summary>
        /// Parse the command-line args
        /// and print Verbs and Options.
        /// </summary>
        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            // Use demo command  line if not argument are provided
            if( args.Length==0) args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------
            
            CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
            Console.WriteLine($"Command-Line: {commandLineArguments.CommandLine}");
            Commander.ExecuteCommand(args);
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main");
        }
    }


    [Command("ConvertToUtc")]
    class ConvertToUtcCommand : CommandBase<ConvertToUtcParameters>
    {
        protected override void Execute(ConvertToUtcParameters ps)
        {
            // Time Zone checking - inline: string to TimeZone
            var localTimeZone=     TimeZoneInfo.FindSystemTimeZoneById(ps.LocalTimeZone);
            Console.WriteLine($"LocalDateTime={ps.LocalDateTime} "+
                              $"in TimeZone '{ps.LocalTimeZone}'");
            DateTime utc = TimeZoneInfo.ConvertTimeToUtc( ps.LocalDateTime, localTimeZone);
            Console.WriteLine($"is UTC: {utc}");
        }



        /// <summary>
        /// OPTIONAL: Error handler to display errors instead of getting an Exception. 
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="handled"></param>
        protected override void OnError(ErrorDetailList errors, bool handled)
        {
            Console.WriteLine(errors.ToString());
            base.OnError(errors, true);
        }
    }



    class ConvertToUtcParameters
    {
        [OptionDescriptor("LocalDateTime", required:true, 
                          helpText:"A local date and time that should be converted into UTC.")]
        public DateTime LocalDateTime { get; set; }
        
        [OptionDescriptor("LocalTimeZone", required:true, 
                          helpText:"Specify the LocalDateTime's time zone")]
        public string LocalTimeZone { get; set; }
    }
}