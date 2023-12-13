using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        //Part1();
        Part2();
    }

    static void Part1()
    {
        string[] engineSchematic = File.ReadAllLines("./example.txt");

        int missingPart = FindMissingPart(engineSchematic);
        Console.WriteLine($"The missing part number is: {missingPart}");
    }

    static void Part2()
    {
        string[] engineSchematic = File.ReadAllLines("./input.txt");

        int gear = FindGear(engineSchematic);
        Console.WriteLine($"The gear number is: {gear}");
    }

    static int FindGear(string[] engineSchematic)
    {
        int sum = 0;
        string[] lines = engineSchematic;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            // A gear is any "*" that has exactly 2 adjacent numbers.
            // Adjacent means either horizontally, vertically, or diagonally.
            
            // regex to find all * in a line
            var matches = Regex.Matches(line, @"\*");

            MatchCollection numbersSameLine = null;
            MatchCollection numbersAboveLine = null;
            MatchCollection numbersBelowLine = null;

            numbersSameLine = Regex.Matches(line, @"\d+");
            if (i > 0)
            {
                numbersAboveLine = Regex.Matches(lines[i - 1], @"\d+");
            }
            if (i < lines.Length - 1)
            {
                numbersBelowLine = Regex.Matches(lines[i + 1], @"\d+");
            }

            // loop through all * in the line
            foreach(Match match in matches)
            {
                var adjacentMatches = new List<Match>();
                foreach (Match number in numbersSameLine)
                {
                    if (MatchIsAdjacentTo(match.Index, number))
                    {
                        adjacentMatches.Add(number);
                    }
                }
                if (numbersAboveLine != null)
                {
                    foreach (Match number in numbersAboveLine)
                    {
                        if (MatchIsAdjacentTo(match.Index, number))
                        {
                            adjacentMatches.Add(number);
                        }
                    }
                }
                if (numbersBelowLine != null)
                {
                    foreach (Match number in numbersBelowLine)
                    {
                        if (MatchIsAdjacentTo(match.Index, number))
                        {
                            adjacentMatches.Add(number);
                        }
                    }
                }

                if (adjacentMatches.Count == 2)
                {
                    sum += int.Parse(adjacentMatches[0].Value) * int.Parse(adjacentMatches[1].Value);
                }
            }
        }
        return sum;
    }

    static bool MatchIsAdjacentTo(int colIndex, Match match)
    {
        var matchColIndex = match.Index;
        var matchColIndexEnd = matchColIndex + match.Length - 1;
        for (int i = matchColIndex; i <= matchColIndexEnd; i++)
        {
            if (colIndex == i || colIndex == i - 1 || colIndex == i + 1)
            {
                return true;
            }
        }
        return false;
    }

    static int FindMissingPart(string[] engineSchematic)
    {
        int sum = 0;
        string[] lines = engineSchematic;
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var matches = Regex.Matches(line, @"\d+");
            foreach (Match match in matches)
            {
                var rowIndex = i;
                var colIndex = match.Index;
                var colIndexEnd = colIndex + match.Length - 1;
                if (LineHasUpperSymbol(lines, rowIndex, colIndex, match.Length) || 
                    LineHasLowerSymbol(lines, rowIndex, colIndex, match.Length) ||
                    LineHasLeftSymbol(lines, rowIndex, colIndex) ||
                    LineHasRightSymbol(lines, rowIndex, colIndexEnd) ||
                    LineHasDiagonalUpperLeftSymbol(lines, rowIndex, colIndex) ||
                    LineHasDiagonalUpperRightSymbol(lines, rowIndex, colIndexEnd) ||
                    LineHasDiagonalLowerLeftSymbol(lines, rowIndex, colIndex) ||
                    LineHasDiagonalLowerRightSymbol(lines, rowIndex, colIndexEnd))
                {
                    sum += int.Parse(match.Value);
                }
            }
        }

        return sum;
    }

    static bool LineHasUpperSymbol(string[] lines, int rowIndex, int colIndex, int length)
    {
        if (rowIndex == 0)
        {
            return false;
        }

        var line = lines[rowIndex - 1];
        for (int i = 0; i < length; i++)
        {
            var symbol = line[colIndex + i];
            if (symbol != '.' && !char.IsDigit(symbol))
            {
                return true;
            }
        }
        return false;
    }

    static bool LineHasLowerSymbol(string[] lines, int rowIndex, int colIndex, int length)
    {
        if (rowIndex == lines.Length - 1)
        {
            return false;
        }

        var line = lines[rowIndex + 1];
        for (int i = 0; i < length; i++)
        {
            var symbol = line[colIndex + i];
            if (symbol != '.' && !char.IsDigit(symbol))
            {
                return true;
            }
        }
        return false;
    }

    static bool LineHasLeftSymbol(string[] lines, int rowIndex, int colIndex)
    {
        var line = lines[rowIndex];
        if (colIndex == 0)
        {
            return false;
        }

        var symbol = line[colIndex - 1];
        return symbol != '.' && !char.IsDigit(symbol);
    }

    static bool LineHasRightSymbol(string[] lines, int rowIndex, int colIndex)
    {
        var line = lines[rowIndex];
        if (colIndex == line.Length - 1)
        {
            return false;
        }

        var symbol = line[colIndex + 1];
        return symbol != '.' && !char.IsDigit(symbol);
    }

    static bool LineHasDiagonalUpperLeftSymbol(string[] lines, int rowIndex, int colIndex)
    {
        if (rowIndex == 0 || colIndex == 0)
        {
            return false;
        }

        var line = lines[rowIndex - 1];
        var symbol = line[colIndex - 1];
        return symbol != '.' && !char.IsDigit(symbol);
    }

    static bool LineHasDiagonalUpperRightSymbol(string[] lines, int rowIndex, int colIndex)
    {
        if (rowIndex == 0)
        {
            return false;
        }

        var line = lines[rowIndex - 1];
        if (colIndex == line.Length - 1)
        {
            return false;
        }

        var symbol = line[colIndex + 1];
        return symbol != '.' && !char.IsDigit(symbol);
    }

    static bool LineHasDiagonalLowerLeftSymbol(string[] lines, int rowIndex, int colIndex)
    {
        if (rowIndex == lines.Length - 1 || colIndex == 0)
        {
            return false;
        }

        var line = lines[rowIndex + 1];
        var symbol = line[colIndex - 1];
        return symbol != '.' && !char.IsDigit(symbol);
    }

    static bool LineHasDiagonalLowerRightSymbol(string[] lines, int rowIndex, int colIndex)
    {
        if (rowIndex == lines.Length - 1)
        {
            return false;
        }

        var line = lines[rowIndex + 1];
        if (colIndex == line.Length - 1)
        {
            return false;
        }

        var symbol = line[colIndex + 1];
        return symbol != '.' && !char.IsDigit(symbol);
    }
}