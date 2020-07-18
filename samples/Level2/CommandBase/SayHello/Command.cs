using System;
using MSPro.CLArgs;






namespace Level2.CommandBaseSample.SayHello
{
    public class Command : CommandBase<CommandParameters>
    {
        protected override void OnExecute(CommandParameters p)
        {
            for (int i = 0; i < p.Count; i++)
            {
                Console.WriteLine($"{i}: Hello {p.Country}");
            }
        }
    }
}