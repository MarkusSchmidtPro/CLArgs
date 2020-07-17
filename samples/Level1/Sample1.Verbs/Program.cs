using System;
using System.Collections.Generic;
using System.Diagnostics;
using MSPro.CLArgs;



namespace Sample1.Verbs
{
    /// <summary>
    ///     Do something with your verbs.
    /// </summary>
    /// <remarks>
    ///     A <c>Verb</c> normally tells your application to <c>do</c> something.
    ///     A <c>Verb</c> specifies which functionality should be executed 
    /// </remarks>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Arguments arguments = CommandLine.Parse(args);
            Console.WriteLine($"Command-Line: {arguments.CommandLine}");
            
            //
            // Functionality: Display arguments
            // [1] = {string} "verb2" View
            Console.WriteLine(">>> Start Functionality");
            // for (int i = 0; i < arguments.Verbs.Count; i++)
            // {
            //     string verb = arguments.Verbs[i];
            //     Console.WriteLine($"Verb[{i}] = '{verb}'");
            // }

            if (arguments.VerbPath == "word1")
                word();
            else if (arguments.VerbPath == "word1.text2")
                text();
            else if (arguments.VerbPath == "word1.text2.verb3")
                verb();
            else
                Debug.Fail("Don't know what to do!");

            Console.WriteLine("<<< End Functionality");
        }



        static void word() => Console.WriteLine("Function Word");
        static void text() => Console.WriteLine("Function Text");
        static void verb() => Console.WriteLine("Function Verb");
    }
}