using System;
using System.Text;
using Microsoft.Extensions.Logging;



namespace MSPro.CLArgs;

public class ConsolePrinter : IPrinter
{
    private readonly ILogger<ConsolePrinter> _logger;


    private readonly StringBuilder _sameLineMessages = new();



    public ConsolePrinter(ILogger<ConsolePrinter> logger)
    {
        _logger = logger;
    }



    public void Debug(string message)
    {
        _logger?.LogDebug(message);
    }



    public void Error(Exception e)
    {
        Error(e.Message);
        _logger?.Log(LogLevel.Critical, e, null);
    }



    public void Write(string message) => Console.Write(message);
    public void WriteLine(string message = null) => Console.WriteLine(message);



    public void Info(string message, bool writeLine = true)
    {
        if (writeLine)
        {
            if (_sameLineMessages.Length > 0)
            {
                _sameLineMessages.Append(message);
                message = _sameLineMessages.ToString();
                _sameLineMessages.Clear();
            }

            _logger?.LogInformation(message);
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
            _sameLineMessages.Append(message);
        }
    }



    public void Warn(string message)
    {
        _logger?.LogWarning(message);
        Console.WriteLine("WARN: " + message);
        _sameLineMessages.Clear();
    }



    public void Error(string message)
    {
        _logger?.LogError(message);
        Console.WriteLine("ERROR: " + message);
        _sameLineMessages.Clear();
    }
}