using System;
using System.Reflection;
using MSPro.CLArgs.Sample3.Command2;



namespace MSPro.CLArgs.Sample3
{
    /// <summary>
    ///     This is the simplest example to parse command-Line arguments.
    /// </summary>
    internal class Program
    {
        private static int Main(string[] args)
        {
            AppReturnCode appResult;

            Console.WriteLine($"*** Start Application '{Assembly.GetExecutingAssembly().GetName().Name}' ***");
            Console.WriteLine($">>> ExecId={AppExecutionProperties.Get().ExecutionId}");
            Console.WriteLine(AssemblyInfo.ToString());
            Console.WriteLine("");
            Console.WriteLine("Parsing command-line arguments ...");

            try
            {
                Arguments arguments = CommandLine.Parse(args);
                Console.WriteLine($"\nCommand-Line: >{arguments.CommandLine}<");

                ICommand2 c = new Command();
                c.Execute(arguments);

                appResult = AppReturnCode.Success;
            }



            #region Exception Handling

            catch (AggregateException ex)
            {
                foreach (Exception iex in ex.InnerExceptions)
                {
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



            Console.WriteLine($"\nApp finished. Result={appResult}");
            Console.WriteLine("\nHit any key to continue...");
            Console.ReadKey();

            return Convert.ToInt32(appResult);
        }



        private enum AppReturnCode
        {
            Success = 0,
            ArgumentValidationFailed = -2,
            AppException = -1
        }
    }
}