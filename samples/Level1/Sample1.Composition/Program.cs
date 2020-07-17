using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;
using MSPro.CLArgs;



namespace Sample1.Composition
{
    /// <summary>
    ///     How to execution functionality with Verbs and Composition.
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Arguments arguments = CommandLine.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            
            // Please refer to Microsoft's documentation to get used to System.Composition
            // Basically all classes annotated with the 'Export' attribute
            // will be collected in the serviceContainer.
            var configuration = new ContainerConfiguration().WithAssembly(Assembly.GetExecutingAssembly());
            var serviceContainer = configuration.CreateContainer();
            
            Console.WriteLine(">>> Start Functionality");
            // No error-handling here. We simply take the first Verb and run a command. 
            var command= serviceContainer.GetExport<IVerbRunner>(arguments.Verbs[0]);
            command.Run( arguments);
            Console.WriteLine("<<< End Functionality");
        }
    }


    [Export("HelloWorld", typeof(IVerbRunner))]
    class HelloWorldCommand : IVerbRunner
    {
        public void Run(Arguments arguments) => Console.WriteLine("This is my HelloWorld command.");
    }   
    
    
    [Export("HelloGermany", typeof(IVerbRunner))]
    class HelloGermanyCommand : IVerbRunner
    {
        public void Run(Arguments arguments) => Console.WriteLine("This is my HelloGermany command.");
    }
    
    
    
    interface IVerbRunner
    {
        void Run(Arguments arguments);
    }
}