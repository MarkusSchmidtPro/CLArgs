using System;
using System.Reflection;
using MSPro.CLArgs.ErrorHandling;



namespace MSPro.CLArgs.Sample2
{
    /// <summary>
    ///     This is the simplest example to parse command-Line arguments.
    /// </summary>
    internal class Program
    {
        enum AppReturnCode
        {
            Success = 0,
            ArgumentValidationFailed = -2,
            AppException = -1
        }



        private static int Main(string[] args)
        {
            // the result that the console app will return when it finishes
            AppReturnCode appResult;    

            // 
            // Print some application information
            //
            Console.WriteLine($"*** Start Application '{Assembly.GetExecutingAssembly().GetName().Name}' ***");
            Console.WriteLine($">>> ExecId={AppExecutionProperties.Get().ExecutionId}");
            Console.WriteLine(AssemblyInfo.ToString());
            Console.WriteLine("");
            Console.WriteLine("Parsing command-line arguments ...");
            
            //
            // Parse command-line and run a command.
            // 
            try
            {
                Arguments arguments = CommandLine.Parse(args);
                Console.WriteLine($"\nCommand-Line: >{arguments.CommandLine}<");

                // Application's functionality is implemented in the
                // form of a command. Both sample commands implement
                // the same functionality. They use different
                // ways to parse the command-line arguments, but the way
                // how a command is being used is always the same - as shown here.

                //ICommand c = new Command1.Command(arguments);
                ICommand c = new Command2.Command(arguments);

                // throwIf:false - Do not throw an AggregateException
                // in case of parsing arguments fails,
                // but print all errors on screen and terminate gracefully.
                c.ValidateAndParseArguments(false);
                if (!c.ValidationErrors.HasErrors())
                {
                    c.Execute();
                    appResult = AppReturnCode.Success;
                }
                else
                {
                    printValidationErrors(c.ValidationErrors);
                    appResult = AppReturnCode.ArgumentValidationFailed;
                }
            }

            #region Exception Handling

            catch (AggregateException ex)
            {
                foreach (Exception iex in ex.InnerExceptions)
                {
                    Console.WriteLine($"ERROR {AppExecutionProperties.Get().ExecutionId} ERROR: {iex.Message}");
                    Console.WriteLine(iex);
                }
                appResult = AppReturnCode.AppException;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                appResult = AppReturnCode.AppException;
            }

            #endregion 

            Console.WriteLine($"\nApp finished.");
            Console.WriteLine($"\nResult={appResult}");
            Console.WriteLine("\nHit any key to continue...");
            Console.ReadKey();

            return Convert.ToInt32(appResult);
        }



        private static void printValidationErrors(ErrorDetailList errors)
        {
            Console.WriteLine($"\t{errors.Details.Count} Argument validation errors");

            foreach (ErrorDetail error in errors.Details)
            {
                foreach (string errorMessage in error.ErrorMessages)
                {
                    Console.WriteLine($"\tArgName:{error.AttributeName} ERR:{errorMessage}");
                }
            }

        }
    }
}