using System;
using System.Collections.Generic;
using MSPro.CLArgs;
using NLog;



namespace CLArgs.Command.CommandCollection.JsonToDBCommand
{
    [Command("JsonToDatabase")]
    class AddCommand : CommandBase<JsonToDbParameters>
    {
        /// <summary>
        /// Do parameter validation.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="unresolvedPropertyNames"></param>
        /// <param name="errors"></param>
        protected override void BeforeExecute(
            JsonToDbParameters parameters,
            HashSet<string> unresolvedPropertyNames, 
            ErrorDetailList errors)
        {
            UsernamePassword up = parameters.UsernamePassword;
            if (up.WindowsAuthentication)
            {
                if( up.Password!=null || up.UserName!=null)
                    errors.AddError("UsernamePassword", "You can specify either WindowsAuthentication OR Username/Password.");
            }
            else // NO Windows Authentication ==> Username AND Password required
            {
                if(  up.UserName==null)
                    errors.AddError("UsernamePassword", "You must specify a Username.");
                if( up.Password==null )
                    errors.AddError("Password", "You must specify a Password.");
            }

            //if (errors.HasErrors()) return;
            // Command will NOT be executed when errors contains messages
        }



        /// <summary>
        /// Custom error-handler to avoid Aggregate Exception in case of errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="handled"></param>
        protected override void OnError(ErrorDetailList errors, bool handled)
        {
            Console.WriteLine(errors.ToString());
            handled = true;
        }



        /// <summary>
        /// Execute Commands Functionality
        /// </summary>
        /// <param name="ps"></param>
        protected override void Execute(JsonToDbParameters ps)
        {
            Console.WriteLine($">><Execute ENTER");
            // do something
            Console.WriteLine($"Username={ps.UsernamePassword.UserName}");
            Console.WriteLine($"Password={ps.UsernamePassword.Password}");
            Console.WriteLine($"WindowsAuthentication={ps.UsernamePassword.WindowsAuthentication}");
            Console.WriteLine($"Filename={ps.Filename}");
            Console.WriteLine($"MaxItems={ps.MaxItems}");

            Console.WriteLine($"<<<Finished EXIT");
        }
    }
}