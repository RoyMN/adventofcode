using AdventOfCode2024.Interfaces;
using AdventOfCode2024.Tools;

namespace AdventOfCode2024
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-usage") || args.Contains("-u"))
            {
                Console.WriteLine("Usage: dotnet run <day> <task> [-s] [-[d/i/w/e]]");
                Console.WriteLine("day: number of the day");
                Console.WriteLine("task: number of the task");
                Console.WriteLine("-s: use sample input (default false)");
                Console.WriteLine("-d: debug log level");
                Console.WriteLine("-i: info log level");
                Console.WriteLine("-w: warning log level (default)");
                Console.WriteLine("-e: error log level");
                Console.WriteLine("-o: override with a specific example. Example must be the last argument\n");
                return;
            }

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run <day> <task> [-s] [-[d/i/w/e]] [-o <example>]");
                Console.WriteLine("[-u]sage for help on usage\n");
                return;
            }

            if (int.TryParse(args[0], out int day) && int.TryParse(args[1], out int task))
            {
                bool sample = args.Contains("-s");
                LogLevel logLevel = LogLevelExtensions.GetLogLevel(args);

                IDailyRunner? runner = TaskGenerator.GetTask(day, task);
                if (runner == null)
                {
                    Console.WriteLine($"Error: Day{day} - Task{task} not implemented\n");
                    return;
                }
                if (args.Contains("-o"))
                {
                    var example = args.Last();
                    Console.WriteLine($"Running example: {example}:\n");
                    Console.WriteLine(runner.RunExample(example, logLevel));
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"Running Day{day} - Task{task}\n");
                    Console.WriteLine(runner.Run(sample, logLevel));
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Usage: dotnet run <day> <task> [-s] [-[d/i/w/e]] [-o <example>]");
                Console.WriteLine("[-u]sage for help on usage\n");
            }
        }
    }
}