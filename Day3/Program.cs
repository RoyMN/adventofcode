using System.Diagnostics;
using Extensions;
using Point = (int x, int y);
using PointedChar = (char ch, (int x, int y) p);
using PointedString = (string s, (int x, int y) p);

var sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("./input1");
var sum = 0;

//lines.DisplayLines();

var stars = lines.FindSymbols('*');

foreach(var star in stars)
{
    //Console.WriteLine($"Star at: {star}");
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
        //Console.WriteLine($"{sum} += {numbers[0].s} * {numbers[1].s}");
        sum += int.Parse(numbers[0].s) * int.Parse(numbers[1].s);
    }
    else
    {
        //Console.WriteLine($"Star has {numbers.Count} adjacent number(s), skipping...");
    }
    //Console.WriteLine();
}
sw.Stop();

Console.WriteLine($"{Environment.NewLine}Total: {sum} found in {sw.ElapsedMilliseconds}ms.");