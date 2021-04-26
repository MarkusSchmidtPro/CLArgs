using System;
using MSPro.CLArgs;



namespace CLArgs.Sample.Options.DefaultCommand
{
    [Command("DemoCommand")]
    class Command : CommandBase<Context>
    {
        protected override void Execute(Context ps)
        {
            Console.WriteLine($"UserName: {ps.DbConnection.UserName}");
            Console.WriteLine($"Password: {ps.DbConnection.Password}");
            Console.WriteLine($"DatabaseTableName: {ps.DatabaseTableName}");
            
            // ps.DatabaseTableNames == null if 'T' is not specified at all
            int count = ps.DatabaseTableNames?.Count ?? 0;

            // DatabaseTableName allows multiple and we iterate over the list:
            Console.WriteLine($"DatabaseTableName specified {count} times");
            for(int i=0; i<count; i++)
            {
                Console.WriteLine($"DatabaseTableName [{i:d2}]: {ps.DatabaseTableNames[i]}");
            }

        }
    }
}