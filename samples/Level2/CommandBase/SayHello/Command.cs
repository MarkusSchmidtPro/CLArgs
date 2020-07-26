using System;
using MSPro.CLArgs;






namespace Level2.CommandBaseSample.SayHello
{
    public class Command : CommandBase<CommandParameters>
    {
        protected override void Execute(CommandParameters ps)
        {
            for (int i = 0; i < ps.Count; i++)
            {
                Console.WriteLine($"{i}: Hello {ps.Country}");
            }
        }
    }
}