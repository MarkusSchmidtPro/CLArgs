using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MSPro.CLArgs.Contract;
using NLog;



namespace MSPro.CLArgs.Sample1
{
    internal class Program
    {
        private static readonly ILogger _log = LogManager.GetCurrentClassLogger();


        private static readonly string[] _optionsTags = {"-", "/", "--"};



        private static void Main(string[] args)
        {
            _log.Info($"*** Start Application '{Assembly.GetExecutingAssembly().GetName().Name}' ***");
            AppExecutionProperties.Get().StartDateUtc = DateTime.UtcNow;
            _log.Info($">>> ExecId={AppExecutionProperties.Get().ExecutionId}");
            _log.Debug(AssemblyInfo.ToString);

            foreach (string s in args)
            {
                _log.Info(s);
            }

            var keyValues = parseArguments(args);
            foreach (KeyValuePair<string, string> kv in keyValues)
            {
                _log.Info($"{kv.Key}={kv.Value}");
            }
        }



        private static Dictionary<string, string> parseArguments(string[] args)
        {
            string arguments = string.Join(' ', args); // issues with --opt2="7 8"

            string optionsTagsFirstChar = _optionsTags.Aggregate(string.Empty, (current, optionsTag) => current + optionsTag[0]);
            _log.Debug($"OptionTags={optionsTagsFirstChar}");

            Dictionary<string, string> result = new Dictionary<string, string>();
            int i = 0;
            while (i < arguments.Length)
            {
                char c = arguments[i];
                if (c == ' ') { i++; }
                else if (optionsTagsFirstChar.Contains(c))
                {
                    KeyValuePair<string, string> option = getOption(arguments, ref i);
                    result[option.Key] = option.Value;
                }
                else if (char.IsLetter(c))
                {
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

            if (arguments[i] == ' ') // no value
            {
                optionValue = "true";
            }
            else
            {
                // skip found char (breaker)
                i++;
                optionValue = getValue(arguments, ref i);
            }

            return new KeyValuePair<string, string>(optionName, optionValue);
        }



        private static string getValue(string arguments, ref int i)
        {
            string optionValue;
            skipBlanks(arguments, ref i);
            char firstChar = arguments[i];
            if( firstChar == '"' || firstChar == '\'')
                optionValue = readString(arguments, ref i);
            else
                optionValue = readUntil(arguments, ref i, new[] {' '});
            
            return optionValue;
        }



        private static string readString(string arguments, ref int i)
        {
            char firstChar = arguments[i];
            i++;    // skip first char " or '

            // don't use Substring but collect all character to support escaping
            List<char> chars = new List<char>();
            while ( arguments[i] != firstChar)
            {
                // skip escape char, collect next char
                if (arguments[i] == '\\')
                {
                    if (++i >= arguments.Length) 
                        throw new ApplicationException($"Unexpected end of string after escape character");
                }
                chars.Add( arguments[i]);
                if (++i >= arguments.Length) 
                    throw new ApplicationException($"Unexpected end of string, Missing closing {firstChar}.");
            }

            i++;    // skip last char
            return string.Join("", chars);
        }
    }
}