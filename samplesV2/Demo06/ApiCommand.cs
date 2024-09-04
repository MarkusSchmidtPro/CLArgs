using MSPro.CLArgs;



namespace Demo06;

[Command("API", "An API Command.")]
public class ApiCommand : CommandBase2<ApiContext>
{
    public ApiCommand(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }



    protected override void Execute()
    {
        Print.Info($"P1      ={_context.P1}");
        Print.Info($"Url     ={_context.Url}");
        Print.Info($"Username={_context.ConnectionParameters.Username}");
        Print.Info($"Password={_context.ConnectionParameters.Password}");
    }
}