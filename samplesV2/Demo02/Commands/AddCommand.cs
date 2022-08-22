using Demo.ICommand2.NET5;
using MSPro.CLArgs;



namespace CLArgs.Demo02;
[Command("MATH.ADD", "Add two integer values.")]
public class AddCommand : CommandBase2<AdditionContext>
{
    public AddCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override void Execute()
    {
        int result = this.Context.Value1 + this.Context.Value2;
        Console.WriteLine($"{this.Context.Value1} + {this.Context.Value2} = {result}");
    }
}