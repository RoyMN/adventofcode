using AdventOfCodeTools;

namespace AdventOfCode2025;

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