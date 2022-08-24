using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using MSPro.CLArgs;

Console.WriteLine("Demo02 - Simple Math");

/*
 * CLArgs Demo02 used the latest .NET 6 top-level Main as the starting point.
 *
 * There are two Commands (Verbs): ADD and SUB,
 * both using the same Context of type <AdditionContext>.
 */

// Test command-lines - note: the first argument is the application's name
//string[] commandline = "MATH ADD /a=5 /b=7".Split(" ").ToArray(); // 5 + 7 = 12
//string[] commandline = "MATH SUB /a=7 /b=5".Split(" ").ToArray();  // 7 - 5 = 2
//string[] commandline = "demo2App.exe".Split(" ").ToArray();  // No Argument --> Help
/*
2 Commands available:
=======================================
Verb                Description
---------------------------------------
SUB                 Subtract two integer values.
ADD                 Add two integer values.
 */

string[] commandline = "MATH ADD /?".Split(" ").ToArray(); // Verb: ADD --> Help on ADD
/*
ADD                 Add two integer values.
--------------------------------------------------------------------------------
/Value1             Tags=a1,a required
                    The first addend or the minuend.
                    DEFAULT: '0'

/Value2             Tags=a2,b required
                    The second addend or the subtrahend.
                    DEFAULT: '0'
 */

//string[] commandline = "MATH ADD /a=5 ".Split(" ").ToArray(); 
// Missing mandatory Option: 'Value2' (Parameter 'Value2')


try
{
    var builder = CommandHostBuilder.Create(commandline);
    builder.Start();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.ReadKey();