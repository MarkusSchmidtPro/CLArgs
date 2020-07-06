using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;



namespace MSPro.CLArgs
{
    internal class Parser
    {
        private readonly char[] _optionsTags;
        private readonly char[] _optionValueTags;


        private int _currentPos;



        public Parser(char[] optionsTags, char[] optionValueTags)
        {
            _optionsTags = optionsTags;
            _optionValueTags = optionValueTags;
        }



        internal Arguments Run(string commandLineArguments)
        {
            Arguments arguments = new Arguments(commandLineArguments);
            _currentPos = 0;
            while (_currentPos < commandLineArguments.Length)
            {
                char c = commandLineArguments[_currentPos];
                if (c == ' ') { _currentPos++; }
                else if (_optionsTags.Any(tag => c == tag))
                {
                    arguments.AddOption(getOption(commandLineArguments));
                }
                else if (char.IsLetter(c))
                {
                    arguments.Verbs.Add(getVerb(commandLineArguments));
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



        private void skipBlanks(string arguments)
        {
            while (_currentPos < arguments.Length && arguments[_currentPos] == ' ') _currentPos++;
        }



        private Option getOption(string arguments)
        {
            Option option = new Option(readUntil(arguments, _optionValueTags));
            if (arguments[_currentPos] != ' ')
            {
                // an option value was provided
                _currentPos++; // skip found char (breaker: between name and value)
                option.Value = getOptionValue(arguments);
            }

            return option;
        }



        private string getOptionValue(string arguments)
        {
            skipBlanks(arguments);
            char firstChar = arguments[_currentPos];

            return firstChar == '"' || firstChar == '\''
                ? readString(arguments)
                : readUntil(arguments, new[] {' '});
        }



        private string readString(string arguments)
        {
            char stringToken = arguments[_currentPos]; // string token " or '
            _currentPos++; // skip token

            // don't use Substring but collect all character to support escaping
            List<char> chars = new List<char>();

            // Iterate until the second token is found
            while (arguments[_currentPos] != stringToken)
            {
                // skip escape char, collect next char
                if (arguments[_currentPos] == '\\')
                {
                    if (++_currentPos >= arguments.Length)
                        throw new ApplicationException("Unexpected end of string after escape character");
                }

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
                .Where(trimmed => !string.IsNullOrWhiteSpace(trimmed) && !trimmed.StartsWith("//") && !trimmed.StartsWith("#"));
        }
    }
}