using System.Text.Json;
using Microsoft.Extensions.Logging;
using MSPro.CLArgs;



/// <summary>
/// MATH MULT2 is the same as MATH MULT, with the difference it uses logging.
/// </summary>
[Command("MATH.MULT2", "Multiply two decimal values.")]
public class Mult2Command : CommandBase2<Mult23Context>
{
    private readonly ILogger<Mult2Command> _logger;

    // Use dependency inject to get an appropriate logger

    public Mult2Command(
        IServiceProvider serviceProvider, 
        ILogger<Mult2Command> logger) : base(serviceProvider)
    {
        _logger = logger;
    }

    protected override void Execute()
    {
        // As defined in nlog.config:
        //   DEBUG messages go to the log file "bin\logs\demo5.log"
        //   INFO  messages are printed on the Console
        _logger.LogDebug($"Serialized Context='{JsonSerializer.Serialize(this.Context)}'");
        decimal result = this.Context.Factor1 * this.Context.Factor2;
        _logger.LogInformation($"{this.Context.Factor1} * {this.Context.Factor2} = {result}");
    }
}