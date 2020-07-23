using System;
using System.Reflection;



namespace MSPro.CLArgs.Sample1
{
    /// <summary>
    /// This is the simplest example to parse command-Line arguments.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"*** Start Application '{Assembly.GetExecutingAssembly().GetName().Name}' ***");
            Console.WriteLine($">>> ExecId={AppExecutionProperties.Get().ExecutionId}");
            Console.WriteLine(AssemblyInfo.ToString());
            Console.WriteLine("");
            Console.WriteLine("Parsing command-line arguments ...");
            try
            {
                Arguments arguments = Commander.ParseCommandLine(args);

                Console.WriteLine($"\nCommand-Line: >{arguments.CommandLine}<");

                Console.WriteLine("\nVerbs:");
                foreach (string verb in arguments.Verbs)
                {
                    Console.WriteLine($"\t{verb}");
                }

                Console.WriteLine("\nOptions:");
                foreach (OptionTag option in arguments.Options.Values)
                {
                    Console.WriteLine($"\t{option.Tag}={option.Value}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("\nHit any key to continue...");
            Console.ReadKey();
        }
    }
}