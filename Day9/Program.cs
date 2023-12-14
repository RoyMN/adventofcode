
var example = false;

//Part1(example);
Part2(example);

void Part1(bool example)
{
    var input = example ? File.ReadAllLines("./day9.example1") : File.ReadAllLines("./day9.input1");

    var newTemps = new List<int>();

    for(int i = 0; i < input.Length; i++)
    {
        var fullSet = new List<List<int>>();
        var history = input[i].Split(' ').Select(int.Parse);
        fullSet.Add([.. history]);
        var reduced = history.All(t => t == 0);
        while (!reduced)
        {
            var next = new List<int>();
            for (int j = 1; j < history.Count(); j++)
            {
                var left = history.ElementAt(j - 1);
                var right = history.ElementAt(j);

                var diff = right - left;
                next.Add(diff);
            }

            reduced = next.All(t => t == 0);
            fullSet.Add([.. next]);
            history = next;
        }

        for (int j = 0; j < fullSet.Count; j++)
        {
            var set = fullSet[j];
            for (int k = 0; k < set.Count; k++)
            {
                Console.Write($"{set[k]} ");
            }
            Console.WriteLine();
        }

        for (int j = fullSet.Count - 2; j >= 0; j--)
        {
            var diff = fullSet[j+1][^1];
            var left = fullSet[j][^1];
            var right = left + diff;

            fullSet[j].Add(right);
        }
        
        newTemps.Add(fullSet[0][^1]);
    }

    Console.WriteLine($"Part 1: {newTemps.Sum()}");
}

void Part2(bool example)
{
    var input = example ? File.ReadAllLines("./day9.example1") : File.ReadAllLines("./day9.input1");

    var newTemps = new List<int>();

    var newInput = new string[input.Length];

    // Read the input backwards for each line
    for (int i = 0; i < input.Length; i++)
    {
        var line = input[i];
        var elements = line.Split(' ');
        var reverseLine = new string(string.Join(' ', elements.Reverse().ToArray()));
        newInput[i] = reverseLine;
    }

    for(int i = 0; i < newInput.Length; i++)
    {
        var fullSet = new List<List<int>>();
        var history = newInput[i].Split(' ').Select(int.Parse);
        fullSet.Add([.. history]);
        var reduced = history.All(t => t == 0);
        while (!reduced)
        {
            var next = new List<int>();
            for (int j = 1; j < history.Count(); j++)
            {
                var left = history.ElementAt(j - 1);
                var right = history.ElementAt(j);

                var diff = right - left;
                next.Add(diff);
            }

            reduced = next.All(t => t == 0);
            fullSet.Add([.. next]);
            history = next;
        }

        for (int j = 0; j < fullSet.Count; j++)
        {
            var set = fullSet[j];
            for (int k = 0; k < set.Count; k++)
            {
                Console.Write($"{set[k]} ");
            }
            Console.WriteLine();
        }

        for (int j = fullSet.Count - 2; j >= 0; j--)
        {
            var diff = fullSet[j+1][^1];
            var left = fullSet[j][^1];
            var right = left + diff;

            fullSet[j].Add(right);
        }
        
        newTemps.Add(fullSet[0][^1]);
    }

    Console.WriteLine($"Part 2: {newTemps.Sum()}");
}