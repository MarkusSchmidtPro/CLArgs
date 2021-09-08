using System;
using CLArgs.Command.CommandCollection.SumCommand;
using MSPro.CLArgs;


namespace CLArgs.Command.CommandCollection.AddCommand
{
    [Command("Math.Add")]
    internal class AddCommand : CommandBase<AddParameters>
    {
        /// <summary>
        ///     Execute Commands Functionality
        /// </summary>
        protected override void Execute(AddParameters ps)
        {
            int sum = ps.Value1 + ps.Value2 + ps.Value3;
            Console.WriteLine($"{ps.Value1}+{ps.Value2}+{ps.Value3} = {sum}");
        }
    }
}