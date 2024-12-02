using Extensions;

var example = false;

Part1();
Part2();

void Part1()
{
    var input = example ? File.ReadAllLines("./example1.txt") :  File.ReadAllLines("./input1.txt");
    var sum = 0;
    for (int i = 0; i < input.Length; i++)
    {
        var line = input[i];
        var lineSum = line.GetFirstAndLastNumberSum();
        sum += lineSum;
    }
    Console.WriteLine($"Part 1: {sum}");
}

void Part2()
{
    var input = example ? File.ReadAllLines("./example2.txt") :  File.ReadAllLines("./input2.txt");
    var sum = 0;
    for (int i = 0; i < input.Length; i++)
    {
        var line = input[i];
        var lineSum = line.GetFirstAndLastNumberSum(true);
        sum += lineSum;
    }
    Console.WriteLine($"Part 2: {sum}");
}