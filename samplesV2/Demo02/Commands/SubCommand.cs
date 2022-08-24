using System;
using MSPro.CLArgs;



namespace CLArgs.Demo02.Commands;

[Command("MATH.SUB", "Subtract two integer values."
                     + "|I have decided to provide a bit more details, to demonstrate explicit line-breaks. "
                     + "together with an extra long text that will be wrapped after 80 characters by default. ")]
public class SubCommand : CommandBase2<AdditionContext>
{
    public SubCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }



    protected override void Execute()
    {
        int result = this.Context.Value1 - this.Context.Value2;
        Console.WriteLine($"{this.Context.Value1} - {this.Context.Value2} = {result}");
    }
}