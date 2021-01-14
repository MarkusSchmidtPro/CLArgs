using System;
using System.Collections.Generic;
using System.Diagnostics;
using MSPro.CLArgs;



namespace CLArgs.Sample.DynamicDefault
{
    internal class Program
    {
        /// <remarks>
        ///     This demo shows you how to validate Parameters and
        ///     how to provide dynamic (e.g. dependent) default values.
        /// </remarks>
        /// <remarks>
        ///     Using [OptionDescriptorAttribute] you can annotate your Command's
        ///     Parameter properties and you can provide _static_ default values.
        ///     This is sufficient for many case. However, parameter validation and/or
        ///     computing dynamic defaults is also a real world requirement.
        ///     In the example, we have a mandatory 'StartDate' and an optional 'EndDate'.
        ///     If 'EndDate' is not provided it should be 'StartDate + 7 days'.
        ///     'StartDate' - in any case must be less than 'EndDate' and greater than '1.1.2020'.
        /// </remarks>
        private const string COMMAND_LINE = "--StartDate=2020-11-01";



        private static void Main(string[] args)
        {
            Console.WriteLine(">>> Start Main");
            Console.WriteLine($"Command-Line: {COMMAND_LINE}");
            args = COMMAND_LINE.Split(' ');
            // ------------------------------------------------

            // No Commander here, simply use the Command directly
            // (its ICommand implementation)!. This is a convenient way
            // if you app has only one command (does not support verbs).

            CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
            var cmd = new FromToCommand();
            cmd.Execute(commandLineArguments);
            
            // ------------------------------------------------
            Console.WriteLine("<<< End Main()");
        }



        [Command("The one an only")]
        private class FromToCommand : CommandBase<FromToCommandParameters>
        {
            /// <summary>
            ///     Check and prepare parameters before execution.
            /// </summary>
            /// <inheritdoc cref="CommandBase{TCommandParameters}.BeforeExecute" />
            protected override void BeforeExecute(FromToCommandParameters ps,
                                                  HashSet<string> unresolvedPropertyNames,
                                                  ErrorDetailList errors)
            {
                //
                // Check K.O. criteria
                //
                DateTime minStartDate = new DateTime(2020, 1, 1);
                if (ps.StartDate < minStartDate)
                {
                    errors.AddError(nameof(ps.StartDate),
                                    $"{nameof(ps.StartDate)} must be greater or equal to {minStartDate:d}, current value: {ps.StartDate:d}");
                }

                // if K.O. (not OK) ==> finish, don't Execute() but OnError()
                if (errors.HasErrors()) return;

                //
                // Complete if necessary:
                // Check if a property's name is in the list of unresolved
                //
                if (unresolvedPropertyNames.Contains(nameof(ps.EndDate)))
                {
                    // Set dynamic, dependent default value
                    Console.WriteLine($"Unresolved {nameof(ps.EndDate)}");
                    ps.EndDate = ps.StartDate.AddDays(7);
                }


                //
                // Check exit conditions
                //    Whatever has been done before: the promise to Execute() is: StartDate < EndDate
                //
                if (!(ps.StartDate < ps.EndDate))
                {
                    errors.AddError(nameof(ps.StartDate),
                                    $"{nameof(ps.StartDate)} ({ps.StartDate:d}) must be less than {nameof(ps.EndDate)} ({ps.EndDate:d}).");
                }
            }



            protected override void Execute(FromToCommandParameters ps)
            {
                // Debug check promises
                Debug.Assert(ps.StartDate >= new DateTime(2020, 1, 1));
                Debug.Assert(ps.StartDate < ps.EndDate);
                
                // Here, when the command executes,
                // FromToCommandParameters is valid and complete.
                Console.WriteLine($"Date Range: {ps.StartDate:d}..{ps.EndDate:d}");
            }



            /// <summary>
            ///     Custom error handler to display error messages instead of throwing exceptions.
            /// </summary>
            /// <inheritdoc cref="CommandBase{TCommandParameters}.OnError" />
            protected override void OnError(ErrorDetailList errors, bool handled)
            {
                Console.WriteLine(errors.ToString());
                base.OnError(errors, true);
            }
        }



        private class FromToCommandParameters
        {
            [OptionDescriptor("StartDate", Required = true)]
            public DateTime StartDate { get; set; }

            [OptionDescriptor("EndDate", Required = false)]
            public DateTime EndDate { get; set; }
        }
    }
}