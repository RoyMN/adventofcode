namespace AdventOfCodeTools;

public class ConsoleWriter(LogLevel logLevel)
{
    public void Info(string message)
    {
        if (logLevel <= LogLevel.INFO)
        {
            Console.WriteLine($"INFO: {message}");
        }
    }

    public void Debug(string message)
    {
        if (logLevel <= LogLevel.DEBUG)
        {
            Console.WriteLine($"DEBUG: {message}");
        }
    }

    public void Warn(string message)
    {
        if (logLevel <= LogLevel.WARN)
        {
            Console.WriteLine($"WARN: {message}");
        }
    }

    public void Error(string message)
    {
        if (logLevel <= LogLevel.ERROR)
        {
            Console.WriteLine($"ERROR: {message}");
        }
    }
}