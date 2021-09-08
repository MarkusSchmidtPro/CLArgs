using System;
using MSPro.CLArgs;



namespace CLArgs.Command.CommandCollection.SumCommand
{
    [Command("Math.Sum")]
    internal class SumCommand : CommandBase<SumParameters>
    {
        /// <summary>
        ///     Execute Commands Functionality
        /// </summary>
        protected override void Execute(SumParameters ps)
        {
            int sum = 0;
            for (int i = 0; i < ps.Values.Count; i++)
            {
                if( i>0) Console.Write("+");
                Console.WriteLine(ps.Values[i]);
                sum += int.Parse( ps.Values[i]);
            }
            Console.WriteLine($"={sum}");
        }
    }
}