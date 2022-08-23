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
        decimal result = this.Context.Factor1 * this.Context.Factor2;
        Console.WriteLine($"{this.Context.Factor1} * {this.Context.Factor2} = {result}");
    }
}