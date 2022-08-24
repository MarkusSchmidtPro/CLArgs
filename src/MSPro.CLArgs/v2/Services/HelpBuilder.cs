using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;



namespace MSPro.CLArgs;

public class HelpBuilder : IHelpBuilder
{
    private readonly ICommandDescriptorCollection _commands;
    private readonly IServiceProvider _serviceProvider;
    private readonly Settings2 _settings;



    public HelpBuilder(IServiceProvider serviceProvider, Settings2 settings, ICommandDescriptorCollection commands)
    {
        _serviceProvider = serviceProvider;
        _settings        = settings;
        _commands        = commands;
    }



    string IHelpBuilder.BuildAllCommandsHelp()
    {
        string insert = new(' ', _settings.HelpAlignColumn);
        StringBuilder sb = new();
        sb.AppendLine($"{_commands.Count} Commands available:");
        sb.AppendLine("=======================================");
        string v = "Verb".PadRight(_settings.HelpAlignColumn);
        sb.AppendLine($"{v}Description");
        sb.AppendLine("---------------------------------------");
        foreach (var commandDescriptor in _commands.Values)
        {
            var wrapped = Helper.Wrap(commandDescriptor.Description, _settings.HelpFullWidth);
            string verbs = commandDescriptor.Verb.Replace('.', ' ');
            sb.AppendLine($"{verbs.PadRight(_settings.HelpAlignColumn)}{wrapped.AllLines[0]}");
            for (int lineNo = 1; lineNo < wrapped.AllLines.Length; lineNo++)
            {
                sb.AppendLine(insert + wrapped.AllLines[lineNo]);
            }
            sb.AppendLine("");
        }
        return sb.ToString();
    }



    string IHelpBuilder.BuildCommandHelp(CommandDescriptor2 c)
    {
        string insert = new(' ', _settings.HelpAlignColumn);

        StringBuilder sb = new();
        sb.AppendLine();
        var wrappedDesc = Helper.Wrap(c.Description, _settings.HelpFullWidth - _settings.HelpAlignColumn);
        string verbs = c.Verb.Replace('.', ' ');
        sb.AppendLine($"{verbs.PadRight(_settings.HelpAlignColumn)}{wrappedDesc.AllLines[0]}");
        for (int lineNo = 1; lineNo < wrappedDesc.AllLines.Length; lineNo++) sb.AppendLine(insert + wrappedDesc.AllLines[lineNo]);

        sb.AppendLine($"{new string('-', _settings.HelpFullWidth)}");

        CommandWithContext  command = (CommandWithContext) _serviceProvider.GetRequiredService(c.Type);
        
        foreach (var option in command.ContextProperties)
        {
            string tags = option.Tags != null ? $"Tags={string.Join(",", option.Tags)} " : string.Empty;
            string required = option.Required ? "required" : "optional" + " ";
            string split = !string.IsNullOrWhiteSpace(option.AllowMultipleSplit)
                ? $" Split='{option.AllowMultipleSplit}'"
                : string.Empty;
            string allowMultiple = !string.IsNullOrWhiteSpace(option.AllowMultiple)
                ? $"AllowMultiple={option.AllowMultiple != null}{split}"
                : string.Empty;
            sb.AppendLine($"/{option.OptionName.PadRight(_settings.HelpAlignColumn - 1)}{tags}{required}{allowMultiple}");
            var wrapped = Helper.Wrap(option.HelpText, _settings.HelpFullWidth - _settings.HelpAlignColumn);
            foreach (string line in wrapped.AllLines) sb.AppendLine(insert + line);
            if (option.Default != null) sb.AppendLine($"{insert}DEFAULT: '{option.Default}'");
        }
        return sb.ToString();
    }
}