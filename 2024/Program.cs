using AdventOfCodeTools;

namespace AdventOfCode2024;

public class TaskGenerator : ITaskGenerator
{
    public IDailyRunner? GetTask(int day, int task)
    {
        return (day, task) switch
        {
            (1, 1) => new Day1.Task1(),
            (1, 2) => new Day1.Task2(),
            (2, 1) => new Day2.Task1(),
            (2, 2) => new Day2.Task2(),
            (3, 1) => new Day3.Task1(),
            (3, 2) => new Day3.Task2(),
            (3, 3) => new Day3WithoutRegex.Task1(),
            (3, 4) => new Day3WithoutRegex.Task2(),
            (4, 1) => new Day4.Task1(),
            (4, 2) => new Day4.Task2(),
            (5, 1) => new Day5.Task1(),
            (5, 2) => new Day5.Task2(),
            (6, 1) => new Day6.Task1(),
            (6, 2) => new Day6.Task2(),
            _ => null
        };
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        var taskGenerator = new TaskGenerator();
        Runner.Main(args, taskGenerator);
    }
}