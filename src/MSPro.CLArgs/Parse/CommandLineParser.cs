using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides the functionality to parse a command-line
    /// </summary>
    /// <see cref="CommandLineParser" />
    public class CommandLineParser
    {
        private readonly Settings _settings;
        private int _currentPos;



        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="settings">
        ///     Provide a <see cref="Settings" /> instance to control the parsing behaviour:
        ///     <see cref="Settings.IgnoreCase" />,
        ///     <see cref="Settings.OptionValueTags" /> and
        ///     <see cref="Settings.OptionsTags" /> are of interest here.
        /// </param>
        public CommandLineParser(Settings settings = null)
        {
            _settings = settings ?? new Settings();
        }



        /// <summary>
        ///     Shortcut and preferred way to use CommandLineParser.
        /// </summary>
        /// <remarks>
        ///     Same as <code>new CommandLineParser(settings).Run(args);</code>.
        /// </remarks>
        /// <seealso cref="CommandLineParser(Settings)" />
        public static Arguments Parse(string[] args, Settings settings = null)
            => new CommandLineParser(settings).Run(args);



        /// <summary>
        ///     Parse a given command-line.
        /// </summary>
        public Arguments Run(string[] args)
        {
            string commandLineArguments = string.Join(" ", args);
            Arguments arguments = new Arguments(commandLineArguments, _settings.IgnoreCase);

            _currentPos = 0;
            while (_currentPos < commandLineArguments.Length)
            {
                char c = commandLineArguments[_currentPos];
                if (c == ' ')
                {
                    _currentPos++;
                }
                else if (_settings.OptionsTags.Any(tag => c == tag))
                {
                    arguments.AddOption(getOption(commandLineArguments));
                }
                else if (char.IsLetter(c))
                {
                    arguments.AddVerb(getVerb(commandLineArguments));
                }
                else
                    throw new ApplicationException($"Unexpected character '{c}' in commandline, pos {_currentPos}.");
            }

            return arguments;
        }



        private string getVerb(string arguments)
            => readUntil(arguments, new[] {' '});



        private string readUntil(string arguments, char[] breaker)
        {
            int startPos = _currentPos;
            while (_currentPos < arguments.Length && !breaker.Contains(arguments[_currentPos])) _currentPos++;
            return arguments.Substring(startPos, _currentPos - startPos);
        }



        private void skipChars(string arguments, char[] skipChars)
        {
            while (_currentPos < arguments.Length && skipChars.Any(sc => sc == arguments[_currentPos])) _currentPos++;
        }



        private Option getOption(string arguments)
        {
            // Name starts at first char that is not an optionsNameIdent
            skipChars(arguments, _settings.OptionsTags);
            Option optionTag = new Option(readUntil(arguments, _settings.OptionValueTags));
            if (arguments.Length > _currentPos && arguments[_currentPos] != ' ')
            {
                // an option value was provided
                _currentPos++; // skip found char (breaker: between name and value)
                optionTag.Value = getOptionValue(arguments);
            }
            else optionTag.Value = true.ToString();

            return optionTag;
        }



        private string getOptionValue(string arguments)
        {
            skipChars(arguments, new[] {' '});
            char firstChar = arguments[_currentPos];

            return firstChar == '"' || firstChar == '\''
                ? readString(arguments)
                : readUntil(arguments, new[] {' '});
        }



        private string readString(string arguments)
        {
            char stringToken = arguments[_currentPos]; // string token " or '
            _currentPos++;                             // skip token

            // don't use Substring but collect all characters to support escaping
            List<char> chars = new List<char>();

            // Iterate until the second token is found
            while (arguments[_currentPos] != stringToken)
            {
                /*
                // msc, 2020-07-15: Escaping removed, kept collecting chars
                // skip escape char, collect next char
                if (arguments[_currentPos] == '\\')
                {
                    if (++_currentPos >= arguments.Length)
                        throw new ApplicationException("Unexpected end of string after escape character");
                }
                */

                chars.Add(arguments[_currentPos]);
                if (++_currentPos >= arguments.Length)
                    throw new ApplicationException($"Unexpected end of string, missing closing {stringToken}.");
            }

            _currentPos++; // skip last char (second string token)
            return string.Join("", chars);
        }



        private static IEnumerable<string> getArgsFromFile(string fileName)
        {
            // filepath starts after '@'
            string filePath = Path.GetFullPath(fileName.Substring(1));
            fileName = Path.GetFileName(filePath);

            if (!File.Exists(filePath))
            {
                // try config directory
                string currentDir = Path.GetDirectoryName(filePath);
                Debug.Assert(currentDir != null, nameof(currentDir) + " != null");
                string configDir = Path.Combine(currentDir, "Config");

                filePath = Path.Combine(configDir, fileName);
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Cannot find config file", fileName);
            }

            string[] lines = File.ReadAllLines(filePath);
            return lines.Select(line => line.Trim())
                        .Where(trimmed => !string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("//") &&
                                          !trimmed.StartsWith("#"));
        }
    }
}