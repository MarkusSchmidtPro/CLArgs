using System;
using System.Collections.Generic;
using System.IO;
using MSPro.CLArgs;
using NLog;



namespace CLArgs.CommandRunner
{
    /// <summary>
    ///     Generic host to run Commands.
    /// </summary>
    /// <remarks>
    ///     Uses NLog for logging.
    ///     dotnetcore 2.1 (on purpose!)
    /// </remarks>
    internal class Program
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();
        
        // USE launchSettings.json configurations to test different scenarios.
        
        private static void Main(string[] args)
        {
            // see nlog.config for logging output.
            // Debug    -->    File crun.log
            // INFO:    -->    Console.Window
            

            // *** YOU MAY WANT TO REFERENCE THOSE PROJECTS
            //     SO THAT THEIR DLLs ARE COPIED
            //     TO THE APPLICATION'S BIN DIRECTORY   ***
            
            //
            // Define the search pattern in which assemblies CLArgs will look for Commands
            //
            const string SEARCH_PATTERN = "CLArgs.Command.*.dll";
            string[] assemblyFileNames = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, SEARCH_PATTERN,
                                                            SearchOption.AllDirectories);

            #region DEBUG and visualisation stuff
            
            _log.Info("*** '{Assembly.GetExecutingAssembly().GetName().Name}' ***");
            _log.Info(AssemblyInfo.ToString());
            _log.Debug($"Searching the following {assemblyFileNames.Length} Assemblies for 'ICommand' implementations");
            foreach (string assemblyFileName in assemblyFileNames)
            {
                _log.Debug(assemblyFileName);
            }

            var resolver = new AssemblyCommandResolver(assemblyFileNames);
            // Just for visualization
            Dictionary<string,Type> commandTypes = resolver.GetCommandTypes();
            _log.Debug($"{commandTypes.Count} Command Pairs found");
            foreach (KeyValuePair<string,Type> keyValuePair in commandTypes)
            {
                _log.Debug($"Verb='{keyValuePair.Key}': Type={keyValuePair.Value.Name}");
            }

            #endregion
            

            Console.WriteLine($"args={string.Join(' ', args)}");
            Console.WriteLine("<<< Start Command()");
            // Let CLArgs resolve the command for the given verb
            Commander.ExecuteCommand(args, new Settings
            {
                AutoResolveCommands = true,
                CommandResolver     = resolver
            });
            Console.WriteLine("<<< End Command()");
        }
    }
}