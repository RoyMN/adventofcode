using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Tools;

public static class TaskGenerator
{
    public static IDailyRunner? GetTask(int day, int task)
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
            _ => null
        };
    }
}