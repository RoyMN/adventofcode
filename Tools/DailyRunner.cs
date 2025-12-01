namespace AdventOfCodeTools;

public interface IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN);
    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN);
}