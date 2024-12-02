// Implement a "runner" that takes parameters from the command line
// and runs the appropriate day and task.
// Parameters are: day as number, task as number, optional flag -s for sample and optional flag -d for debug.
// Example: dotnet run 1 1 -s -d
using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run <day> <task> [-s] [-[d/i/w/e]] [-o <example>]");
                Console.WriteLine("-? for help");
                return;
            }

            if (args.Contains("-?"))
            {
                Console.WriteLine("Usage: dotnet run <day> <task> [-s] [-[d/i/w/e]]");
                Console.WriteLine("day: number of the day");
                Console.WriteLine("task: number of the task");
                Console.WriteLine("-s: use sample input (default false)");
                Console.WriteLine("-d: debug log level");
                Console.WriteLine("-i: info log level");
                Console.WriteLine("-w: warning log level (default)");
                Console.WriteLine("-e: error log level");
                Console.WriteLine("-o: override with a specific example. Example must be the last argument");
                return;
            }

            if (int.TryParse(args[0], out int day) && int.TryParse(args[1], out int task))
            {
                bool sample = args.Contains("-s");
                LogLevel logLevel = LogLevelExtensions.GetLogLevel(args);

                IDailyRunner runner = TaskGenerator.GetTask(day, task);
                if (args.Contains("-o"))
                {
                    var example = args.Last();
                    Console.WriteLine(runner.RunExample(example, logLevel));
                }
                else
                {
                    Console.WriteLine(runner.Run(sample, logLevel));
                }
            }
            else
            {
                Console.WriteLine("Usage: dotnet run <day> <task> [-s] [-[d/i/w/e]] [-o <example>]");
                Console.WriteLine("-? for help");
            }
        }
    }
}