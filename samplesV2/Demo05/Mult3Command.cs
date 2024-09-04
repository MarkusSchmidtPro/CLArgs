using System.Text.Json;
using Microsoft.Extensions.Logging;
using MSPro.CLArgs;



/// <summary>
/// MATH MULT2 is the same as MATH MULT, with the difference it uses printing.
/// </summary>
/// <see cref="IPrinter"/>
/// <see cref="ConsolePrinter"/>
[Command("MATH.MULT3", "Multiply two decimal values.")]
public class Mult3Command : CommandBase2<Mult23Context>
{
    public Mult3Command(IServiceProvider serviceProvider) : base(serviceProvider) { }

    protected override void Execute()
    {
        Print.Debug($"Serialized Context='{JsonSerializer.Serialize(_context)}'");
        decimal result = _context.Factor1 * _context.Factor2;
        Print.Info($"{_context.Factor1} * {_context.Factor2} = {result}");
    }
}