using System.Collections.Generic;
using MSPro.CLArgs;
using NLog;



namespace CLArgs.Command.CommandCollection.JsonToDBCommand
{
    [Command("JsonToDatabase")]
    class JsonToDbCommand : CommandBase<JsonToDbParameters>
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Do parameter validation.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="unresolvedPropertyNames"></param>
        /// <param name="errors"></param>
        protected override void BeforeExecute(JsonToDbParameters ps, HashSet<string> unresolvedPropertyNames, ErrorDetailList errors)
        {
            UsernamePassword up = ps.UsernamePassword;
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
            _log.Error(errors.ToString);
            handled = true;
        }



        /// <summary>
        /// Execute Commands Functionality
        /// </summary>
        /// <param name="ps"></param>
        protected override void Execute(JsonToDbParameters ps)
        {
            _log.Info($">><Execute ENTER");
            // do something
            _log.Info($"Username={ps.UsernamePassword.UserName}");
            _log.Info($"Password={ps.UsernamePassword.Password}");
            _log.Info($"WindowsAuthentication={ps.UsernamePassword.WindowsAuthentication}");
            _log.Info($"Filename={ps.Filename}");
            _log.Info($"MaxItems={ps.MaxItems}");
            
            _log.Info($"<<<Finished EXIT");
        }
    }
}