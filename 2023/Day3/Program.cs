using System.Diagnostics;
using Extensions;
using Point = (int x, int y);
using PointedChar = (char ch, (int x, int y) p);
using PointedString = (string s, (int x, int y) p);

Part1();
Part2();

void Part1()
{
    var sw = Stopwatch.StartNew();
    var lines = File.ReadAllLines("./input1");
    var sum = 0;

    //lines.DisplayLines();

    var symbols = lines.FindSymbols(null);

    foreach(var symbol in symbols)
    {
        var adjacentChars = lines.AdjacentChars(symbol).Where(ch => char.IsDigit(ch.ch));
        var numbers = new List<PointedString>();

        foreach(var ch in adjacentChars)
        {
            if (!numbers.Any(n => n.Contains(ch.p)))
            {
                var stringNumber = lines[ch.p.y].StringNumber(ch.p);
                numbers.Add(stringNumber);
            }
        }


        foreach (var (s, _) in numbers)
        {
            sum += int.Parse(s);
        }
    }
    sw.Stop();

    Console.WriteLine($"{Environment.NewLine}Part 1: {sum} found in {sw.ElapsedMilliseconds}ms.");
}

void Part2()
{
    var sw = Stopwatch.StartNew();
    var lines = File.ReadAllLines("./input1");
    var sum = 0;

    var stars = lines.FindSymbols('*');

    foreach(var star in stars)
    {
        var adjacentChars = lines.AdjacentChars(star).Where(ch => char.IsDigit(ch.ch));
        var numbers = new List<PointedString>();

        foreach(var ch in adjacentChars)
        {
            if (!numbers.Any(n => n.Contains(ch.p)))
            {
                var stringNumber = lines[ch.p.y].StringNumber(ch.p);
                numbers.Add(stringNumber);
            }
        }


        if (numbers.Count == 2)
        {
            sum += int.Parse(numbers[0].s) * int.Parse(numbers[1].s);
        }
    }
    sw.Stop();

    Console.WriteLine($"{Environment.NewLine}Part 2: {sum} found in {sw.ElapsedMilliseconds}ms.");
}