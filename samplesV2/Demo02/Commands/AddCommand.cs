using System;
using MSPro.CLArgs;



namespace CLArgs.Demo02.Commands;
[Command("MATH.ADD", "Add two integer values.")]
public class AddCommand(IServiceProvider serviceProvider) : CommandBase2<AdditionContext>(serviceProvider)
{
    protected override void Execute()
    {
        int result = _context.Value1 + _context.Value2;
        Console.WriteLine($"{_context.Value1} + {_context.Value2} = {result}");
    }
}