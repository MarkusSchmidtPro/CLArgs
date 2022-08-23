using System;
using MSPro.CLArgs;



namespace CLArgs.Demo02;

[Command("MATH SUB", "Subtract two integer values.")]
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