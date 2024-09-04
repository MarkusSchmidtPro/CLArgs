using System.Text.Json;
using MSPro.CLArgs;



/// <summary>
/// MATH MULT2 is the same as MATH MULT, with the difference it uses printing.
/// </summary>
[Command("MATH.MULT3", "Multiply two decimal values.")]
public class Mult3Command(IServiceProvider serviceProvider) : CommandBase2<Mult23Context>(serviceProvider)
{
    protected override void Execute()
    {
        Console.WriteLine($"Serialized Context='{JsonSerializer.Serialize(_context)}'");
        decimal result = _context.Factor1 * _context.Factor2;
        Console.WriteLine($"{_context.Factor1} * {_context.Factor2} = {result}");
    }
}