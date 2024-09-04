using System;
using MSPro.CLArgs;



namespace CLArgs.Demo02.Commands;

[Command("MATH.SUB", "Subtract two integer values."
                     + "|I have decided to provide a bit more details, to demonstrate explicit line-breaks. "
                     + "together with an extra long text that will be wrapped after 80 characters by default. ")]
public class SubCommand(IServiceProvider serviceProvider) : CommandBase2<AdditionContext>(serviceProvider)
{
    protected override void Execute()
    {
        int result = this._context.Value1 - this._context.Value2;
        Console.WriteLine($"{this._context.Value1} - {this._context.Value2} = {result}");
    }
}