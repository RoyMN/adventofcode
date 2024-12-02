using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Interfaces;

public interface IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN);

    /**
     * RunExample is used to run a specific example. Implement when needed.
     * @param example: string - the example string, must have a new line
     * @param logLevel: LogLevel - the log level
     * @return string - the result of the example
     */
    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN);
}