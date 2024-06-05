using System.Data;
using Extensions;
using static System.Console;
using static Extensions.Extensions;

var input = File.ReadAllText("./input").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

//DisplayLines(expandedInput);

var symbols = input.ListSymbols('#');

var combinations = symbols.ListSymbolCombinations();
//DisplayLines(combinations);
WriteLine($"Total combinations: {combinations.Count}");

var expandedRows = input.ExpandedRows();
var expandedColumns = input.ExpandedColumns();

var totalDistance = combinations.Sum(comb => comb.ExpandedDistance(expandedColumns, expandedRows, 1000000));

DisplayLine($"Total distance: {totalDistance}");
