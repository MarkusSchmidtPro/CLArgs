using System.Text.Json;
using Microsoft.Extensions.Logging;
using MSPro.CLArgs;



/// <summary>
/// MATH MULT2 is the same as MATH MULT, with the difference it uses printing.
/// </summary>
[Command("MATH.MULT3", "Multiply two decimal values.")]
public class Mult3Command : CommandBase2<Mult23Context>
{
    public Mult3Command(IServiceProvider serviceProvider) : base(serviceProvider) { }

    protected override void Execute()
    {
        Print.Debug($"Serialized Context='{JsonSerializer.Serialize(this.Context)}'");
        decimal result = this.Context.Factor1 * this.Context.Factor2;
        Print.Info($"{this.Context.Factor1} * {this.Context.Factor2} = {result}");
    }
}