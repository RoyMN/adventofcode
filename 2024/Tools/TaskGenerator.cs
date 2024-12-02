using System.Reflection.Metadata.Ecma335;
using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Tools;

public static class TaskGenerator
{
    public static IDailyRunner GetTask(int day, int task)
    {
        return (day, task) switch
        {
            (1, 1) => new Day1.Task1(),
            (1, 2) => new Day1.Task2(),
            (2, 1) => new Day2.Task1(),
            (2, 2) => new Day2.Task2(),
            _ => throw new NotImplementedException($"Task not implemented day: {day}, task: {task}")
        };
    }
}