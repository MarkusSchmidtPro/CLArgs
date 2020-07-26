using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.Options.DefaultCommand
{
    [Command("DemoCommand")]
    class Command : CommandBase<Parameters>
    {
        protected override void Execute(Parameters ps)
        {
            Console.WriteLine($"UserName: {ps.DbConnection.UserName}");
            Console.WriteLine($"Password: {ps.DbConnection.Password}");
            Console.WriteLine($"DatabaseTableName: {ps.DatabaseTableName}");
        }
    }
}