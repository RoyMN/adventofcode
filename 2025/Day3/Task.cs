using AdventOfCodeTools;

namespace AdventOfCode2025.Day3;

public class Task1 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        long sum = 0;
        string fileName = sample ? "./2025/Day3/sample.txt" : "./2025/Day3/input.txt";
        var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        var line = sr.ReadLine() ?? throw new Exception("Input file is empty");
        var parts = new char[] { char.MinValue, char.MinValue };
        while (line != null)
        {
            parts[0] = char.MinValue;
            parts[1] = char.MinValue;
            for (int i = 0; i < line.Length; i++)
            {
                char digit = line[i];
                if (digit > parts[0] && i < line.Length - 1)
                {
                    parts[1] = char.MinValue;
                    parts[0] = digit;
                } else if (digit > parts[1])
                {
                    parts[1] = digit;
                }
            }
            logger.Info($"Processed line: {line}");
            logger.Info($"Line processed. Max1: {parts[0]}, Max2: {parts[1]}");
            line = sr.ReadLine();
            sum += int.Parse($"{parts[0]}{parts[1]}");
            logger.Info($"Current sum: {sum}");
        }
        return $"Som of largest 2-digit numbers from each line: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        // Implementation for running examples goes here
        throw new NotImplementedException();
    }
}

public class Task2 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        long sum = 0;
        int n = 12;
        string fileName = sample ? "./2025/Day3/sample.txt" : "./2025/Day3/input.txt";
        var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        var line = sr.ReadLine() ?? throw new Exception("Input file is empty");
        char[] parts = new char[n];
        while (line != null)
        {
            for (int p = 0; p < parts.Length; p++) parts[p] = char.MinValue;

            var lastUsedIndex = -1;
            var lineLength = line.Length;
            var searchScopeSize = lineLength - n;
            logger.Info($"Processing line: {line}");
            for (int i = 0; i < n; i++)
            {
                var endIndex = searchScopeSize + i;
                var startIndex = lastUsedIndex + 1;
                var length = endIndex - startIndex + 1;
                if (length <= 0)
                {
                    throw new InvalidOperationException("Not enough characters to select the required digits.");
                }

                string searchScope = line.Substring(startIndex, length);
                logger.Info($"Processing scope: {searchScope}");
                var idx = -1;
                var maxChar = char.MinValue;
                for (int j = 0; j < searchScope.Length; j++)
                {
                    if (searchScope[j] > maxChar)
                    {
                        maxChar = searchScope[j];
                        idx = j;
                    }
                }
                parts[i] = maxChar;
                lastUsedIndex = startIndex + idx;
                logger.Info($"Part {i} processed. Max char: {maxChar} at index {lastUsedIndex}");
            }
            logger.Info($"Line processed. Parts: {new string(parts)}");
            sum += long.Parse(new string(parts));
            logger.Info($"Current sum: {sum}");
            line = sr.ReadLine();
        }
        return $"Som of largest {n}-digit numbers from each line: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        // Implementation for running examples goes here
        throw new NotImplementedException();
    }
}

public class Task3 : IDailyRunner
{
    public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
    {
        long sum = 0;
        int n = 12;
        string fileName = sample ? "./2025/Day3/sample.txt" : "./2025/Day3/input.txt";
        var sr = new StreamReader(fileName);
        var logger = new ConsoleWriter(logLevel);
        var line = sr.ReadLine() ?? throw new Exception("Input file is empty");
        while (line != null)
        {
            var stack = new char[n];
            int stackSize = 0;
            int lineLen = line.Length;
            logger.Info($"Processing line: {line}");
            for (int i = 0; i < lineLen; i++)
            {
                char ch = line[i];
                int remaining = lineLen - i;
                // backtrack while we can pop smaller elements and still have enough characters left
                while (stackSize > 0 && stack[stackSize - 1] < ch && (stackSize - 1 + remaining) >= n)
                {
                    stackSize--;
                }
                if (stackSize < n)
                {
                    stack[stackSize++] = ch;
                }
            }
            if (stackSize < n)
            {
                throw new InvalidOperationException("Not enough characters to select the required digits.");
            }
            var resultStr = new string(stack, 0, n);
            logger.Info($"Line processed. Parts: {resultStr}");
            sum += long.Parse(resultStr);
            logger.Info($"Current sum: {sum}");
            line = sr.ReadLine();
        }
        return $"Som of largest {n}-digit numbers from each line: {sum}";
    }

    public string RunExample(string example, LogLevel logLevel = LogLevel.WARN)
    {
        throw new NotImplementedException();
    }
}