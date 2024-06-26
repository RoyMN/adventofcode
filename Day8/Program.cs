﻿using Extensions;

var example = false;

//Part1();
Part2();

void Part1()
{
    var input = example ? File.ReadAllLines("./Day8.example1") : File.ReadAllLines("./Day8.input");
    var instructions = input[0].Select(x => ParseDirection(x)).ToArray();
    (string startKey, var map) = ParseMap(input[2..]);
    // Apparently startkey is always AAA and not the first in the list
    // var steps = Traverse("AAA", map, instructions);
    // Console.WriteLine($"Part 1: {steps}");
}

void Part2()
{
    var input = example ? File.ReadAllLines("./Day8.example3") : File.ReadAllLines("./Day8.input");
    var instructions = input[0];
    (string[] startKeys, var map) = ParseMapWithStartKeys(input[2..]);
    var steps = startKeys.Select(key => WalkKey(key, map, String.Join("",instructions))).ToList();
    //Console.WriteLine($"Part 2: Steps found:");
    foreach (var step in steps)
    {
        Console.WriteLine(step);
    }
    var res = steps.Aggregate(MappingExtensions.lcm);
    Console.WriteLine($"Part 2: Steps found: {res}");
}

int WalkKeys(string[] keys, Dictionary<string, (string L, string R)> map, string instructions)
{
    for (int i = 0; i < keys.Length; i++)
    {
        var key = keys[i];
        var steps = WalkKey(key, map, instructions);
        Console.WriteLine($"Key: {key}, steps: {steps}");
    }
}

int WalkKey(string key, Dictionary<string, (string L, string R)> map, string instructions)
{
    var step = 0;
    string? firstZ = null;
    var currentKey = key;
    var found = false;
    while(!found)
    {
        var instruction = instructions.GetInstruction(step);
        // Console.WriteLine($"Current key: {currentKey}, instruction: {instruction}, step: {step}");
        var nextKey = instruction switch
        {
            'L' => map[currentKey].L,
            'R' => map[currentKey].R,
            _ => throw new Exception("Invalid direction"),
        };
        if (nextKey[2] == 'Z')
        {
            if (firstZ == null)
            {
                firstZ = nextKey;
            }
            else if (firstZ == nextKey)
            {
                found = true;
                // Console.WriteLine("Found a cycle!");
            }
        }
        currentKey = nextKey;
        step++;
    }
    return step / 2;
}

/*
int Traverse(string startKey, Dictionary<string, (string L, string R)> map, Direction[] instructions, int steps = 0)
{
    var currentSteps = steps;
    var currentKey = startKey;
    if (currentKey == "ZZZ") return currentSteps;
    for (int i = 0; i < instructions.Length; i++)
    {
        var instruction = instructions[i];
        var (L, R) = map[currentKey];
        currentKey = instruction switch
        {
            Direction.L => L,
            Direction.R => R,
            _ => throw new Exception("Invalid direction"),
        };
        currentSteps++;
        if (currentKey == "ZZZ")
        {
            return currentSteps;
        }
    }
    return Traverse(currentKey, map, instructions, currentSteps);
}
*/

/*
int TraverseFromMultiKeys(string[] startKeys, Dictionary<string, (string L, string R)> map, Direction[] instructions, Dictionary<(string[], Direction), string[]> cache, int steps = 0)
{
    Mapper mapper = new(startKeys, map, instructions);
    while (!mapper.Valid)
    {
        // Console.WriteLine($"Current keys: {string.Join(", ", mapper.currentKeys)}");
        mapper.Next();
    }
    return mapper.Steps;
}
*/

string ParseDirection(char direction)
{
    return direction switch
    {
        'L' => "L",
        'R' => "R",
        _ => throw new Exception("Invalid direction"),
    };
}

(string start, Dictionary<string, (string L, string R)>) ParseMap(string[] input)
{
    var map = new Dictionary<string, (string L, string R)>();

    // Do first line outside of loop so we can extract key
    var _line = input[0];
    var _parts = _line.Split(" = ");
    var _key = _parts[0];
    (var _L, var _R) = _parts[1][1..^1].Split(", ");
    map.Add(_key, (_L, _R));
    for (int i = 1; i < input.Length; i++)
    {
        var line = input[i];
        var parts = line.Split(" = ");
        var key = parts[0];
        (var L, var R) = parts[1][1..^1].Split(", ");
        map.Add(key, (L, R));
    }
    return (_key, map);
}

(string[] startKeys, Dictionary<string, (string L, string R)>) ParseMapWithStartKeys(string[] input)
{
    var map = new Dictionary<string, (string L, string R)>();
    var _startKeys = new List<string>();
    for (int i = 0; i < input.Length; i++)
    {
        var line = input[i];
        var parts = line.Split(" = ");
        var key = parts[0];
        (var L, var R) = parts[1][1..^1].Split(", ");
        map.Add(key, (L, R));
        if (key[2] == 'A')
        {
            // All keys ending with A are start keys
            _startKeys.Add(key);
        }
    }
    return (_startKeys.ToArray(), map);
}