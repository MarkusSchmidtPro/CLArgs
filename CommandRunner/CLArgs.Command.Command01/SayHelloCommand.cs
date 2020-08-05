using System;
using MSPro.CLArgs;



namespace CLArgs.Command.Command01
{
    [Command("SayHello")]
    public class SayHelloCommand : CommandBase<SayHelloParameters>
    {
        protected override void Execute(SayHelloParameters ps)
        {
            for (int i = 0; i < ps.Count; i++)
            {
                Console.WriteLine($"{i}: Hello {ps.Country}");
            }
        }
    }
}