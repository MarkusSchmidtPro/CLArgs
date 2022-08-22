using CLArgs.Demo02;
using MSPro.CLArgs;



namespace Demo.ICommand2.NET5;

[Command("MATH.SUB", "Subtract two integer values.")]
public class SubCommand : CommandBase2<AdditionContext>
{
    public SubCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }



    protected override void Execute()
    {
        int result = Context.Value1 - Context.Value2;
        Console.WriteLine($"{Context.Value1} - {Context.Value2} = {result}");
    }
}