using Extensions;
using Utils;

// Part1();
Part2();

void Part1()
{
    var input = File.ReadAllLines("./input");

    var records = input.ToConditionRecord();

    var res = records.Select(r => r.Springs.GenerateCombinations().Where(c => {
        ConditionRecord cr = new(c, r.DamagedGroups);
        return cr.IsValidConditionRecord();
        })
    ).Select(r => r.Count()).ToArray();

    res.Sum().Display();
}

void Part2()
{
    var input = File.ReadAllLines("./input");

    var records = input.ToConditionRecord().Select(r => r.UnfoldFiveTimes()).ToArray();

    var res = records.Select(r =>
    {
        List<char> chars = [.. r.Springs];
        List<int> numbs = [.. r.DamagedGroups];

        var intGen = new InterpretationGenerator(chars, numbs);

        return intGen.CountValidInterpretations(new State(0, 0, 0, ""));
    }).ToList();

    res.Sum().Display();
}