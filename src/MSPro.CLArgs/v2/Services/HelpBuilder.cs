using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

public class HelpBuilder(IServiceProvider serviceProvider, Settings2 settings, ICommandDescriptorCollection commands)
    : IHelpBuilder
{
    string IHelpBuilder.BuildAllCommandsHelp()
    {
        string insert = new(' ', settings.HelpAlignColumn);
        StringBuilder sb = new();
        sb.AppendLine($"{commands.Count} Commands available:");
        sb.AppendLine("=======================================");
        string v = "Verb".PadRight(settings.HelpAlignColumn);
        sb.AppendLine($"{v}Description");
        sb.AppendLine("---------------------------------------");
        foreach (CommandDescriptor2 commandDescriptor in commands.Values.OrderBy( c=> c.Verb))
        {
            Helper.WrappedText wrapped = Helper.Wrap(commandDescriptor.Description, settings.HelpFullWidth);
            string verbs = commandDescriptor.Verb.Replace('.', ' ');
            sb.AppendLine($"{verbs.PadRight(settings.HelpAlignColumn)}{wrapped.AllLines[0]}");
            for (var lineNo = 1; lineNo < wrapped.AllLines.Length; lineNo++)
            {
                sb.AppendLine(insert + wrapped.AllLines[lineNo]);
            }

            sb.AppendLine("");
        }

        return sb.ToString();
    }



    string IHelpBuilder.BuildCommandHelp(CommandDescriptor2? commandDescriptor)
    {
        string insert = new(' ', settings.HelpAlignColumn);

        StringBuilder sb = new();
        sb.AppendLine();
        Helper.WrappedText wrappedDesc = Helper.Wrap(commandDescriptor.Description, settings.HelpFullWidth - settings.HelpAlignColumn);
        string verbs = commandDescriptor.Verb.Replace('.', ' ');
        sb.AppendLine($"{verbs.PadRight(settings.HelpAlignColumn)}{wrappedDesc.AllLines[0]}");
        for (var lineNo = 1; lineNo < wrappedDesc.AllLines.Length; lineNo++) sb.AppendLine(insert + wrappedDesc.AllLines[lineNo]);

        sb.AppendLine($"{new string('-', settings.HelpFullWidth)}");
        var command = (CommandWithContext)serviceProvider.GetRequiredService(commandDescriptor.Type);
        foreach (ContextProperty option in command.ContextProperties) //.OrderBy( cp => cp.))
        {
            string tags = option.Tags != null ? $"Tags={string.Join(",", option.Tags)} " : string.Empty;
            string required = option.Required ? "required" : "optional" + " ";
            string split = !string.IsNullOrWhiteSpace(option.AllowMultipleSplit)
                ? $" Split='{option.AllowMultipleSplit}'"
                : string.Empty;
            string allowMultiple = !string.IsNullOrWhiteSpace(option.AllowMultiple)
                ? $"AllowMultiple={option.AllowMultiple != null}{split}"
                : string.Empty;
            sb.AppendLine($"/{option.OptionName.PadRight(settings.HelpAlignColumn - 1)}{tags}{required}{allowMultiple}");
            Helper.WrappedText wrapped = Helper.Wrap(option.HelpText, settings.HelpFullWidth - settings.HelpAlignColumn);
            foreach (string line in wrapped.AllLines) sb.AppendLine(insert + line);
            if (option.Default != null) sb.AppendLine($"{insert}DEFAULT: '{option.Default}'");
        }

        return sb.ToString();
    }
}