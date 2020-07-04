using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MSPro.CLArgs.Contract;
using NLog;



namespace MSPro.CLArgs.Sample1
{
    internal class Program
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();





        private static void Main(string[] args)
        {
            _log.Info($"*** Start Application '{Assembly.GetExecutingAssembly().GetName().Name}' ***");
            AppExecutionProperties.Get().StartDateUtc = DateTime.UtcNow;
            _log.Info($">>> ExecId={AppExecutionProperties.Get().ExecutionId}");
            _log.Debug(AssemblyInfo.ToString);

            foreach (string s in args)
            {
                _log.Info(s);
            }

            var keyValues = parseArguments(args);
            foreach (KeyValuePair<string, string> kv in keyValues)
            {
                _log.Info($"{kv.Key}={kv.Value}");
            }
        }



    }
}