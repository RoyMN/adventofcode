namespace AdventOfCode2024.Tools;

public static class LogLevelExtensions
{
    public static LogLevel GetLogLevel(string[] input)
    {
        if (input.Contains("-d")) return LogLevel.DEBUG;
        if (input.Contains("-i")) return LogLevel.INFO;
        if (input.Contains("-w")) return LogLevel.WARN;
        if (input.Contains("-e")) return LogLevel.ERROR;
        return LogLevel.WARN;
    }
}

public enum LogLevel
{
    DEBUG,
    INFO,
    WARN,
    ERROR
}