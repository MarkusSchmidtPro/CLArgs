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
        Print.Info($"P1      ={this.Context.P1}");
        Print.Info($"Url     ={this.Context.Url}");
        Print.Info($"Username={this.Context.ConnectionParameters.Username}");
        Print.Info($"Password={this.Context.ConnectionParameters.Password}");
    }
}