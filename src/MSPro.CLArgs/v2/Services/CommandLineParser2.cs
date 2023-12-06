using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides the functionality to parse a command-line.
    /// </summary>
    internal class CommandLineParser2
    {
        private const string FIRST_CHAR_PATTERN = "^[A-Za-z]";
        private const string WORD_PATTERN = "[A-Za-z0-9_\\-]*";
        private readonly Regex _optionNameRegEx = new($"(\\?)|({FIRST_CHAR_PATTERN}{WORD_PATTERN})");
        private readonly Regex _verbRegEx = new($"({FIRST_CHAR_PATTERN}{WORD_PATTERN})");

    
        private readonly Settings2 _settings;

        internal CommandLineParser2( Settings2 settings)
        {
            _settings    = settings;
        }


        private bool isVerb(string arg) => _verbRegEx.Match(arg).Value == arg;

        /// <summary>
        /// Get the option tag as it was specified in the command-line.
        /// </summary>
        private string getOptionTag(string arg) => _optionNameRegEx.Match(arg).Value;



        /// <summary>
        ///     Parse command-line arguments into an argument collection.
        /// </summary>
        public void Parse(IReadOnlyList<string> args, IArgumentCollection arguments)
        {
            //
            // Get Verbs
            //
            int argNo = 0;
            while (argNo < args.Count && isVerb(args[argNo]))
            {
                arguments.Add(Argument.Verb(args[argNo]));
                argNo++;
            }

            //
            // Argument [argNo] is no longer a verb
            // Get options
            //
            Argument lastOption = null; // the last option that has been recognized

            while (argNo < args.Count)
            {
                string arg = args[argNo];
                bool isOption = false; // current argument is an option
                foreach (string optionTag in _settings.OptionsTags)
                {
                    if (!arg.StartsWith(optionTag)) continue;

                    isOption = true;
                    arg      = arg[optionTag.Length..];
                    break;
                }

                if (isOption)
                {
                    //
                    // current arg (args[argNo]) should be an option!
                    //
                    string optionTag  = getOptionTag(arg);
                    if (string.IsNullOrWhiteSpace(optionTag))
                        throw new ApplicationException($"Unexpected Command-Line Argument: {args[argNo]}.");

                    lastOption = Argument.Option(optionTag, true.ToString());
                    arguments.Add(lastOption);

                    // skip Option name and continue parsing
                    arg = arg[optionTag.Length..].Trim();
                    if (arg.Length > 0)
                        // The current argument may continue (without any 'interrupt')
                        // with an optionValueTag and [optional] the option's value
                        // "/Package=" ,
                        // "2021-04.03_Sprint03\\iPack.jsonc" ,
                        // e.g. /PackageDir=Packages
                        foreach (string optionValueTag in _settings.OptionValueTags)
                        {
                            if (!arg.StartsWith(optionValueTag)) continue;

                            string optionValue = arg[optionValueTag.Length..];
                            //  if (optionValue.Length > 0) {
                            // 2021-12-02: Must allow zero string options (length=0): /dn=""
                            lastOption.Value = optionValue;
                            lastOption       = null; // option is complete!
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
                        foreach (string optionValueTag in _settings.OptionValueTags)
                        {
                            if (!arg.StartsWith(optionValueTag)) continue;
                            arg = arg[optionValueTag.Length..];
                            break;
                        }

                        lastOption.Value = arg;
                        lastOption       = null; // option is complete
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
                            Parse(fileArgs, arguments);
                        }
                        else
                        {
                            arguments.Add(Argument.Target(args[argNo]));
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
        private static IEnumerable<string> readArgs(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            var argumentLines = lines.Select(line => line.Trim())
                .Where(trimmed => !string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("//") &&
                                  !trimmed.StartsWith("#"));
            string commandLine = string.Join(" ", argumentLines);
            return Helper.SplitCommandLine(commandLine);
        }
    }
}