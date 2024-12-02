using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Day1
{
    public class Task1 : IDailyRunner
    {
        public string Run(bool sample = false, LogLevel logLevel = LogLevel.WARN)
        {
            string fileName = sample ? "./Day1/sample.txt" : "./Day1/input.txt";
            StreamReader sr = new(fileName);
            ConsoleWriter logger = new(logLevel);
            string? line = sr.ReadLine();
            int[] sLeft = [];
            int[] sRight = [];
            while (line != null)
            {
                (int left, int right) = line.ToIntTouple("   ");
                sLeft = sLeft.InsertSort(left);
                sRight = sRight.InsertSort(right);
                line = sr.ReadLine();
            }

            logger.Debug($"Left input: {string.Join(' ', sLeft)}");
            logger.Debug($"Right input: {string.Join(' ', sRight)}");

            var sumDistances = 0;

            for (int i = 0; i < sLeft.Length; i++)
            {
                sumDistances += Math.Abs(sLeft[i] - sRight[i]);
            }

            return $"Sum of distances: {sumDistances}";
        }
    }
}