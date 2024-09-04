using MSPro.CLArgs;



namespace Demo06;

[Command("API", "An API Command.")]
public class ApiCommand(IServiceProvider serviceProvider) : CommandBase2<ApiContext>(serviceProvider)
{
    protected override void Execute()
    {
        Console.WriteLine($"P1      ={_context.P1}");
        Console.WriteLine($"Url     ={_context.Url}");
        Console.WriteLine($"Username={_context.ConnectionParameters.Username}");
        Console.WriteLine($"Password={_context.ConnectionParameters.Password}");
    }



    protected override void AfterExecute(ErrorDetailList errors)
    {
        if (errors.Details.Count > 0) Console.WriteLine(errors.ToString());
        base.AfterExecute(errors);
    }
}