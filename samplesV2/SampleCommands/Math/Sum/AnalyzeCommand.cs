using Microsoft.Extensions.DependencyInjection;
using MSPro.CLArgs;



namespace SampleCommands.Math.Sum;

[Command("MATH.ANALYZE", "Analyze any number of integer values.")]
public class AnalyzeCommand(IServiceProvider serviceProvider) : CommandBase2<SumContext>(serviceProvider)
{
    /* Alternative way of using DependencyInjection
    private readonly IArgumentCollection _argumentsAlternativeWayOfUsingThem;
    public AnalyzeCommand(IServiceProvider serviceProvider, IArgumentCollection argumentsAlternativeWayOfUsingThem) : base(serviceProvider)
    {
        _argumentsAlternativeWayOfUsingThem = argumentsAlternativeWayOfUsingThem;
    }
    */



    protected override void Execute()
    {
        IArgumentCollection arguments = ServiceProvider.GetRequiredService<IArgumentCollection>();
        //IArgumentCollection arguments = _argumentsAlternativeWayOfUsingThem;
        for (int i = 0; i < arguments.Count; i++)
        {
            Argument argument = arguments[i];
            switch (argument.Type)
            {
                case ArgumentType.Verb:
                    Console.WriteLine($"{i} {argument.Type}  : {argument.Key}");
                    break;
                case ArgumentType.Option:
                    Console.WriteLine($"{i} {argument.Type}: {argument.Key}={argument.Value}");
                    break;
                case ArgumentType.Target:
                    Console.WriteLine($"{i} {argument.Type}: {argument.Value}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}