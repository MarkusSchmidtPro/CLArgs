using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;



namespace MSPro.CLArgs
{
    /// <summary>
    ///     Provides the functionality to parse a command-line
    /// </summary>
    /// <see cref="CommandLineParser" />
    public class CommandLineParser
    {
        private readonly Settings _settings;



        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="settings">
        ///     Provide a <see cref="Settings" /> instance to control the parsing behaviour:
        ///     <see cref="Settings.IgnoreCase" />,
        ///     <see cref="Settings.OptionValueTags" /> and
        ///     <see cref="Settings.OptionsTags" /> are of interest here.
        /// </param>
        private CommandLineParser(Settings settings = null)
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
        private Arguments Run(string[] args)
        {
            string commandLineArguments = string.Join(" ", args);
            Arguments arguments = new Arguments(commandLineArguments, _settings.IgnoreCase);

            var sp = new StringParser(_settings);
            sp.Parse(commandLineArguments, arguments);

            return arguments;
        }



        private class StringParser
        {
            private readonly Settings _settings;
            private string _argumentsString;
            private int _currentPos;



            public StringParser([NotNull] Settings settings)
            {
                _settings = settings;
            }



            /// <summary>
            ///     Parse a string containing arguments
            /// </summary>
            internal void Parse(string argumentsString, Arguments arguments)
            {
                _currentPos      = 0;
                _argumentsString = argumentsString;

                while (_currentPos < _argumentsString.Length)
                {
                    char c = _argumentsString[_currentPos];
                    if (c == ' ')
                    {
                        _currentPos++;
                    }
                    else if (_settings.OptionsTags.Any(tag => c == tag))
                    {
                        arguments.SetOption(getOption());
                    }
                    else if (c == '@')
                    {
                        readFromFile( arguments);
                    }
                    else if (char.IsLetter(c))
                    {
                        arguments.AddVerb(getVerb());
                    }
                    else
                        throw new ApplicationException(
                            $"Unexpected character '{c}' in commandline, pos {_currentPos}.");
                }
            }



            private void readFromFile(Arguments arguments)
            {
                string fileName = getFileName();
                var args = getArgsFromFile(fileName);
                StringParser sp = new StringParser(_settings);
                sp.Parse(string.Join(" ", args),arguments);
            }



            private string getFileName()
            {
                char firstChar = _argumentsString[_currentPos];
                return firstChar == '"' || firstChar == '\''
                    ? readString()
                    : readUntil(Path.GetInvalidPathChars().Concat( Path.GetInvalidFileNameChars()).ToArray());
            }

            private static IEnumerable<string> getArgsFromFile(string fileName)
            {
                string filePath = Path.GetFullPath(fileName.Substring(1));
                fileName = Path.GetFileName(filePath);
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("Cannot find config file", fileName);

                string[] lines = File.ReadAllLines(filePath);
                return lines.Select(line => line.Trim())
                            .Where(trimmed => !string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("//") &&
                                              !trimmed.StartsWith("#"));
            }



            private Option getOption()
            {
                // Name starts at first char that is not an optionsNameIdent
                skipChars( _settings.OptionsTags);
                Option optionTag = new Option(readUntil( _settings.OptionValueTags));
                if (_argumentsString.Length > _currentPos && _argumentsString[_currentPos] != ' ')
                {
                    // an option value was provided
                    _currentPos++; // skip found char (breaker: between name and value)
                    optionTag.Value = getOptionValue();
                }
                else optionTag.Value = true.ToString();
                return optionTag;
            }



            private string getVerb() => readUntil( new[] {' '});



            private string readUntil(char[] breaker)
            {
                int startPos = _currentPos;
                while (_currentPos < _argumentsString.Length && !breaker.Contains(_argumentsString[_currentPos]))
                    _currentPos++;
                return _argumentsString.Substring(startPos, _currentPos - startPos);
            }



            private string readAll(char[] validChars)
            {
                int startPos = _currentPos;
                while (_currentPos < _argumentsString.Length
                       && validChars.Contains(_argumentsString[_currentPos])) _currentPos++;
                return _argumentsString.Substring(startPos, _currentPos - startPos);
            }



            private void skipChars(char[] skipChars)
            {
                while (_currentPos < _argumentsString.Length
                       && skipChars.Any(sc => sc == _argumentsString[_currentPos]))
                    _currentPos++;
            }



            private string getOptionValue()
            {
                skipChars(new[] {' '});
                char firstChar = _argumentsString[_currentPos];
                return firstChar == '"' || firstChar == '\''
                    ? readString()
                    : readUntil(new[] {' '});
            }



            private string readString()
            {
                char stringToken = _argumentsString[_currentPos]; // string token " or '
                _currentPos++;                                    // skip token

                // don't use Substring but collect all characters to support escaping
                List<char> chars = new List<char>();

                // Iterate until the second token is found
                while (_argumentsString[_currentPos] != stringToken)
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

                    chars.Add(_argumentsString[_currentPos]);
                    if (++_currentPos >= _argumentsString.Length)
                        throw new ApplicationException($"Unexpected end of string, missing closing {stringToken}.");
                }

                _currentPos++; // skip last char (second string token)
                return string.Join("", chars);
            }
        }
    }
}