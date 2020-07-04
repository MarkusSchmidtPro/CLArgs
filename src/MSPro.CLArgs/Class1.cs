using System;
using System.Collections.Generic;
using System.Linq;



namespace MSPro.CLArgs
{
    public static class Parser
    {
        /// <summary>
        ///     A command-line argument that starts with any of these character
        ///     is considered to be an <c>Option</c>.
        /// </summary>
        public static char[] OptionsTags { get; set; } = {'-', '/'};



        /// <summary>
        ///     Parse the command-line arguments into a key-value list.
        /// </summary>
        /// <returns>
        ///     Command-line arguments as key-value list.
        ///     Key is the option's name and value is the provided value as a string.<br />
        /// </returns>
        public static Dictionary<string, string> Parse(string[] args)
        {
            string arguments = string.Join(" ", args);
            Dictionary<string, string> result = new Dictionary<string, string>();
            int i = 0;

            while (i < arguments.Length)
            {
                char c = arguments[i];
                if (c == ' ') { i++; }
                else if (OptionsTags.Any( tag=> c==tag))
                {
                    // Recognized the start of an option
                    KeyValuePair<string, string> option = getOption(arguments, ref i);
                    result[option.Key] = option.Value;
                }
                else if (char.IsLetter(c))
                {
                    // recognized the start of a verb
                    string verb = getVerb(arguments, ref i);
                    result[verb] = null;
                }
                else
                    throw new ApplicationException($"Unexpected character '{c}' in commandline, pos {i}.");
            }

            return result;
        }



        private static string getVerb(string arguments, ref int i)
            => readUntil(arguments, ref i, new[] {' '});



        private static string readUntil(string arguments, ref int i, char[] breaker)
        {
            int startPos = i;
            while (i < arguments.Length && !breaker.Contains(arguments[i])) i++;
            return arguments.Substring(startPos, i - startPos);
        }



        private static void skipBlanks(string arguments, ref int i)
        {
            while (i < arguments.Length && arguments[i] == ' ') i++;
        }



        private static KeyValuePair<string, string> getOption(string arguments, ref int i)
        {
            string optionName = readUntil(arguments, ref i, new[] {' ', ':', '='});
            string optionValue;

            if (arguments[i] == ' ') 
            {
                // no value, option is set: true
                optionValue = "true";
            }
            else
            {
                i++; // skip found char (breaker: between name and value)
                optionValue = getOptionValue(arguments, ref i);
            }

            return new KeyValuePair<string, string>(optionName, optionValue);
        }



        private static string getOptionValue(string arguments, ref int i)
        {
            skipBlanks(arguments, ref i);
            char firstChar = arguments[i];

            return firstChar == '"' || firstChar == '\'' 
                ? readString(arguments, ref i) 
                : readUntil(arguments, ref i, new[] {' '});
        }



        private static string readString(string arguments, ref int i)
        {
            char stringToken = arguments[i];    // string token " or '
            i++;                                // skip token

            // don't use Substring but collect all character to support escaping
            List<char> chars = new List<char>();

            // Iterate until the second token is found
            while (arguments[i] != stringToken)
            {
                // skip escape char, collect next char
                if (arguments[i] == '\\')
                {
                    if (++i >= arguments.Length)
                        throw new ApplicationException("Unexpected end of string after escape character");
                }

                chars.Add(arguments[i]);
                if (++i >= arguments.Length)
                    throw new ApplicationException($"Unexpected end of string, missing closing {stringToken}.");
            }

            i++; // skip last char (second string token)
            return string.Join("", chars);
        }
    }
}