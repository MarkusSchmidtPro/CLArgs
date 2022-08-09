﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace MSPro.CLArgs;

/// <summary>
///     The top level class to easily use 'CLArgs'.
/// </summary>
[PublicAPI]
public class Commander2
{
    public Commander2(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
  
    }

    private readonly ServiceProvider _serviceProvider;

    
    public void ExecuteCommand()
    {
        var commandlineArguments = _serviceProvider.GetRequiredService<ICommandlineArgumentCollection>();
        var commandDescriptors = _serviceProvider.GetRequiredService<ICommandDescriptorCollection>();

        if (commandDescriptors == null || commandDescriptors.Count == 0)
            throw new ApplicationException("No Commands have been registered");
/*
          if (!Verbs.Any())
          {
              if (!Targets.Any() &&
                  (Options.Count == 0 || commandLineArguments.HelpRequested))
              {
                  _settings.DisplayAllCommandsDescription?.Invoke(CommandDescriptors);
                  return;
              }

              // No explicit Verb specified in command-line
              // set default verb
              commandLineArguments.AddVerb("DEFAULT");
          }
*/

        string verbPath = !commandlineArguments.Verbs.Any() ? null : string.Join(".", commandlineArguments.Verbs);
        var commandDescriptor = commandDescriptors[verbPath];

        /*   if (commandDescriptor == null
               && Verbs.Any()
               && Verbs[0].StartsWith("clargs", _settings.StringComparison))
               commandDescriptor = ResolveCommand(commandLineArguments.Verbs[0]);
 
           if (.HelpRequested)
           {
               _settings.DisplayCommandHelp(commandDescriptor);
               return;
           }*/


/*        var commands =  _serviceProvider.CreateAsyncScope()<ICommand2>();
        var command= commands.First(c => c.GetType() == commandDescriptor.Type);
*/
        IServiceScopeFactory scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var command = (ICommand2)_serviceProvider.GetService(commandDescriptor.Type);
        command.Execute();
    }
}