using System;
using System.IO;
using MSPro.CLArgs;
using NLog;



namespace CLArgs.ConsoleHost
{
    /// <summary>
    ///     Generic host to run Commands.
    /// </summary>
    /// <remarks>
    ///     Uses NLog for logging.
    /// </remarks>
    internal class Program
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            // see nlog.config for logging output.
            // Debug    -->    File crun.log
            // INFO:    -->    Console.Window
            _log.Debug(AssemblyInfo.ToString());

            // *** YOU MAY WANT TO REFERENCE THOSE PROJECTS
            //     SO THAT THEIR DLLs ARE COPIED
            //     TO THE APPLICATION'S BIN DIRECTORY   ***

            //
            // Define the search pattern in which assemblies CLArgs will
            // look for Commands: Classes annotated with "[Command()]" attribute
            //
            string[] assemblyFileNames = Directory.GetFiles(
                AppDomain.CurrentDomain.BaseDirectory,
                "CLArgs.Command.*.dll",
                SearchOption.AllDirectories);

            var resolver = new AssemblyCommandResolver(assemblyFileNames);


            #region DEBUG and visualisation

#if DEBUG
            _log.Debug($"Searching the following {assemblyFileNames.Length} Assemblies for 'ICommand' implementations");
            foreach (string assemblyFileName in assemblyFileNames)
            {
                _log.Debug(assemblyFileName);
            }

            // Just for visualization
            var commandDescriptors = resolver.GetCommandDescriptors();
            _log.Debug($"{commandDescriptors.Count} Command Pairs found");
            foreach (var keyValuePair in commandDescriptors)
            {
                var commandType = keyValuePair.CreateCommandInstance();
                _log.Debug($"Verb='{keyValuePair.Verb}': Type={commandType.GetType()}");
            }

            Console.WriteLine(AssemblyInfo.ToString());
            Console.WriteLine($"args={string.Join(' ', args)}");
            Console.WriteLine("<<< Start Command()");
#endif

            #endregion

            // Open and view or edit
            //      Properties\launchSettings.json
            // to test different scenarios.

            Commander.ExecuteCommand(args, new Settings
            {
                AutoResolveCommands = true,
                IgnoreCase = true,
                CommandResolver = resolver
            });
        }
    }
}