using System.Diagnostics;
using Utils;

var example = false;

Part1();
Part2();

void Part1()
{
    var input = example ? File.ReadAllLines("./day7-example1") : File.ReadAllLines("./day7-input1");
    var sw = Stopwatch.StartNew();
    var sum = input.Solve<Card>(false);
    sw.Stop();
    Console.WriteLine($"Found answer: {sum} in {sw.ElapsedMilliseconds}ms");
}


void Part2()
{
    var input = example ? File.ReadAllLines("./day7-example1") : File.ReadAllLines("./day7-input1");
    var sw = Stopwatch.StartNew();
    var sum = input.Solve<JCard>(false);
    sw.Stop();
    Console.WriteLine($"Found answer: {sum} in {sw.ElapsedMilliseconds}ms");
}

