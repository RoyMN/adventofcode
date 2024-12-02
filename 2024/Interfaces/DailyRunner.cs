using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Interfaces;

public interface IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN);
    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN);
}