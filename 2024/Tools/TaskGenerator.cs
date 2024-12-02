using AdventOfCode2024.Interfaces;

namespace AdventOfCode2024.Tools;

public static class TaskGenerator
{
    public static IDailyRunner GetTask(int day, int task)
    {
        if (day == 1 && task == 1)
        {
            return new Day1.Task1();
        }
        throw new NotImplementedException($"Task not found: day {day} task {task}");
    }
}