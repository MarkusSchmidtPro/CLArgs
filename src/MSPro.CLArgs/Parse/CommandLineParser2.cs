using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MSPro.CLArgs;

/// <summary>
///     Provides the functionality to parse a command-line.
/// </summary>
public class CommandLineParser2
{
    /// <summary>
    ///     Shortcut and preferred way to use CommandLineParser.
    /// </summary>
    /// <remarks>
    ///     Same as <code>new CommandLineParser(settings).Run(args);</code>.
    /// </remarks>
    public static CommandLineArguments2 Parse(string[] args, Settings2 settings)
    {
        CommandLineArguments2 commandLineArguments = new( settings);
        CommandLineParser2 sp = new(settings.OptionsTags, settings.OptionValueTags);
        sp.Parse(args, commandLineArguments);
        return commandLineArguments;
    }


    #region CommandLineParser

    private readonly string[] _optionsTags;
    private readonly string[] _optionValueTags;
    private const string FIRST_CHAR_PATTERN = "^[A-Za-z]";
    private const string WORD_PATTERN = "[A-Za-z0-9_\\-]*";
    private readonly Regex _verbRegEx = new($"({FIRST_CHAR_PATTERN}{WORD_PATTERN})");
    private readonly Regex _optionNameRegEx = new($"(\\?)|({FIRST_CHAR_PATTERN}{WORD_PATTERN})");

    /// <summary>
    ///     Create a new instance.
    /// </summary>
    /// <param name="optionsTags">
    ///     <see cref="Settings.OptionsTags" />
    /// </param>
    /// <param name="optionValueTags">
    ///     <see cref="Settings.OptionValueTags" />
    /// </param>
    private CommandLineParser2(string[] optionsTags, string[] optionValueTags)
    {
        _optionsTags = optionsTags;
        _optionValueTags = optionValueTags;
    }


    private bool isVerb(string arg) => _verbRegEx.Match(arg).Value == arg;

    private string getOptionName(string arg) => _optionNameRegEx.Match(arg).Value;


    /// <summary>
    ///     Parse a string containing arguments
    /// </summary>
    private void Parse(IReadOnlyList<string> args, CommandLineArguments2 commandLineArguments)
    {
        int argNo = 0;
        while (argNo < args.Count && isVerb(args[argNo]))
        {
            commandLineArguments.AddVerb(args[argNo]);
            argNo++;
        }

        // Argument [argNo] is no longer a verb

        Option lastOption = null; // the last option that has been recognized

        while (argNo < args.Count)
        {
            string arg = args[argNo];
            bool isOption = false; // current argument is an option
            foreach (string optionTag in _optionsTags)
            {
                if (!arg.StartsWith(optionTag)) continue;

                isOption = true;
                arg = arg.Substring(optionTag.Length);
                break;
            }

            if (isOption)
            {
                //
                // current arg (args[argNo]) should be an option!
                //
                string optionName = getOptionName(arg);
                if (string.IsNullOrWhiteSpace(optionName))
                    throw new ApplicationException(
                        $"Unexpected Command-Line Argument: {args[argNo]}.");

                lastOption = new Option(optionName, true.ToString());
                commandLineArguments.AddOption(lastOption);

                // skip Option name and continue parsing
                arg = arg.Substring(optionName.Length).Trim();
                if (arg.Length > 0)
                    // The current argument may continue (without any 'interrupt')
                    // with an optionValueTag and [optional] the option's value
                    // "/Package=" ,
                    // "2021-04.03_Sprint03\\iPack.jsonc" ,
                    // e.g. /PackageDir=Packages
                    foreach (string optionValueTag in _optionValueTags)
                    {
                        if (!arg.StartsWith(optionValueTag)) continue;

                        string optionValue = arg.Substring(optionValueTag.Length);
                        //  if (optionValue.Length > 0) {
                        // 2021-12-02: Must allow zero string options (length=0): /dn=""
                        lastOption.Value = optionValue;
                        lastOption = null; // option is complete!
                        break;
                        // }
                    }
            }
            else
            {
                // current argument is not an Option
                // can be an option value (even starting with an OptionValueTag)
                // or if there is no 'pending' option, we consider it to be a Target or a file
                if (lastOption != null)
                {
                    // skip option tag values
                    foreach (string optionValueTag in _optionValueTags)
                    {
                        if (!arg.StartsWith(optionValueTag)) continue;
                        arg = arg.Substring(optionValueTag.Length);
                        break;
                    }

                    lastOption.Value = arg;
                    lastOption = null; // option is complete
                }
                else
                {
                    if (arg.StartsWith("@"))
                    {
                        // file
                        string fileName = arg.Substring("@".Length);
                        string filePath = Helper.FindFile(fileName,
                            Environment.CurrentDirectory,
                            Helper.BinDir);

                        string[] fileArgs = readArgs(filePath).ToArray();
                        CommandLineParser2 sp = new(_optionsTags, _optionValueTags);
                        sp.Parse(fileArgs, commandLineArguments);
                    }
                    else
                    {
                        commandLineArguments.AddTarget(args[argNo]);
                    }
                }
            }

            // ---------- next argument -----------
            argNo++;
        }
    }


    /// <summary>
    ///     Read arguments from file. Uses Windows CommandLineToArgvW().
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private static string[] readArgs(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        var argumentLines = lines.Select(line => line.Trim())
            .Where(trimmed => !string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("//") &&
                              !trimmed.StartsWith("#"));
        string commandLine = string.Join(" ", argumentLines);
        string[] args = Helper.SplitCommandLine(commandLine);
        return args;
    }

    #endregion
}