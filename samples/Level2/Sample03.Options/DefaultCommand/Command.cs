using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.Options.DefaultCommand
{
    [Command("DemoCommand")]
    class Command : CommandBase<Parameters>
    {
        protected override void OnExecute(Parameters p)
        {
            Console.WriteLine($"UserName: {p.DbConnection.UserName}");
            Console.WriteLine($"Password: {p.DbConnection.Password}");
            Console.WriteLine($"DatabaseTableName: {p.DatabaseTableName}");
        }
    }
}