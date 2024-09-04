using System;
using System.Text;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs
{
    public class ConsolePrinter(ILogger<ConsolePrinter> logger) : IPrinter
    {
        private readonly StringBuilder _sameLineMessages = new();



        public void Debug(string message)
        {
            logger?.LogDebug(message);
        }



        public void Error(Exception e)
        {
            Error(e.Message);
            logger?.Log(LogLevel.Critical, e, null);
        }



        public void Write(string message) => Console.Write(message);
        public void WriteLine(string? message = null) => Console.WriteLine(message);



        public void Info(string message, bool writeLine = true)
        {
            if (writeLine)
            {
                // Previous messages have been written to console already,
                // so write only the latest message.
                Console.WriteLine(message);
            
                // Previous messages have to be combined for logging,
                // because they haven't been logged yet.
                if (_sameLineMessages.Length > 0)
                {
                    _sameLineMessages.Append(message);
                    message = _sameLineMessages.ToString();
                    _sameLineMessages.Clear();
                }

                logger?.LogInformation(message);
            
            }
            else
            {
                Console.Write(message);
                _sameLineMessages.Append(message);
            }
        }



        public void Warn(string message)
        {
            logger?.LogWarning(message);
            Console.WriteLine("WARN: " + message);
            _sameLineMessages.Clear();
        }



        public void Error(string message)
        {
            logger?.LogError(message);
            Console.WriteLine("ERROR: " + message);
            _sameLineMessages.Clear();
        }
    }
}