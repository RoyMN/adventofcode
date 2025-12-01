namespace AdventOfCodeTools;

public interface ITaskGenerator
{
    public IDailyRunner? GetTask(int day, int task);
}