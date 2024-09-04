using MSPro.CLArgs;



namespace SampleCommands.Math.Mult;


[Command("MATH.MULT", "Multiply two decimal values.")]
public class MultCommand : CommandBase2<MultContext>
{
    public MultCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override void Execute()
    {
        decimal result = _context.Factor1 * _context.Factor2;
        Console.WriteLine($"{_context.Factor1} * {_context.Factor2} = {result}");
    }
}