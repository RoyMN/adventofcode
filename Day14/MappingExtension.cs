namespace Extensions;

using Point = (int col, int row);

public static class MappingExtensions
{
    public static char[] Column(this string[] input, int colIdx)
    {
        return input.Select(row => row[colIdx]).ToArray();
    }

    public static int Roll(this Point p, List<Point> cubeRocks, List<Point> roundRocks, int height)
    {
        var topRow = 0;
        // Find first cube rock in the same column
        // that is between the current rock and the top of the map
        var cubeRock = cubeRocks
            .Where(r => r.col == p.col)
            .Where(r => r.row < p.row)
            .OrderByDescending(r => r.row)
            .FirstOrDefault();

        if (cubeRock != default)
        {
            topRow = cubeRock.row + 1;
        }

        // Find amount of round rocks between the current rock and the top of the map
        var roundRocksCount = roundRocks
            .Where(r => r.row != p.row) // Excluding the current rock
            .Where(r => r.col == p.col)
            .Where(r => r.row < p.row)
            .Where(r => r.row >= topRow)
            .Count();
        
        return height - (topRow + roundRocksCount);
    }

    public static int NorthLoad(this List<Point> cubeRocks, int height)
    {
        var sum = 0;
        foreach(var rock in cubeRocks)
        {
            sum += height - rock.row;
        }
        return sum;
    }


    /// <summary>
    /// Returns a string representation (hash) of the list of rocks
    /// </summary>
    /// <param name="roundRocks"></param>
    /// <returns></returns>
    public static string GetHash(this List<Point> roundRocks)
    {
        return string.Join(";", roundRocks.OrderBy(p => p.col).ThenBy(p => p.row).Select(p => $"{p.col},{p.row}"));
    }

    public static List<Point> TiltNorth(this List<Point> roundRocks, List<Point> cubeRocks)
    {
        roundRocks = [.. roundRocks.OrderBy(r => r.row)];
        var result = new List<Point>();

        foreach (var rock in roundRocks)
        {
            var topRow = 0;
            // Find rocks in same column from result set
            var _rr = result.Where(r => r.col == rock.col).ToList();       
            
            // Find cube rocks in the same column
            var _cr = cubeRocks.Where(r => r.col == rock.col).ToList();

            // Find highest row from _rr and _cr between the current rock and the top of the map
            var collisions = _rr
                .Concat(_cr)
                .Where(r => r.row < rock.row)
                .OrderByDescending(r => r.row);
            
            if (collisions.Any())
            {
                var (col, row) = collisions.First();
                topRow = row + 1;
            }

            // Add rock to result set with the new row
            result.Add((rock.col, topRow));
        }

        return result;
    }

    public static List<Point> TiltWest(this List<Point> roundRocks, List<Point> cubeRocks)
    {
        roundRocks = [.. roundRocks.OrderBy(r => r.col)];
        var result = new List<Point>();

        foreach (var rock in roundRocks)
        {
            var westCol = 0;

            // Find rocks in same row from result set
            var _rr = result.Where(r => r.row == rock.row).ToList();       
            
            // Find cube rocks in the same row
            var _cr = cubeRocks.Where(r => r.row == rock.row).ToList();

            // Find highest col from _rr and _cr between the current rock and the west of the map
            var collisions = _rr
                .Concat(_cr)
                .Where(r => r.col < rock.col)
                .OrderByDescending(r => r.col);
            
            if (collisions.Any())
            {
                var (col, row) = collisions.First();
                westCol = col + 1;
            }

            // Add rock to result set with the new row
            result.Add((westCol, rock.row));
        }
        return result;
    }

    public static List<Point> TiltSouth(this List<Point> roundRocks, List<Point> cubeRocks, int southIdx)
    {
        roundRocks = [.. roundRocks.OrderByDescending(r => r.row)];
        var result = new List<Point>();

        foreach (var rock in roundRocks)
        {
            var southRow = southIdx;

            // Find rocks in same column from result set
            var _rr = result.Where(r => r.col == rock.col).ToList();

            // Find cube rocks in the same column
            var _cr = cubeRocks.Where(r => r.col == rock.col).ToList();

            // Find lowest row from _rr and _cr between the current rock and the south of the map
            var collisions = _rr
                .Concat(_cr)
                .Where(r => r.row > rock.row)
                .OrderBy(r => r.row);

            if (collisions.Any())
            {
                var (col, row) = collisions.First();
                southRow = row - 1;
            }

            // Add rock to result set with the new row
            result.Add((rock.col, southRow));
        }
        return result;
    }

    public static List<Point> TiltEast(this List<Point> roundRocks, List<Point> cubeRocks, int eastIdx)
    {
        roundRocks = [.. roundRocks.OrderByDescending(r => r.col)];
        var result = new List<Point>();

        foreach (var rock in roundRocks)
        {
            var eastCol = eastIdx;

            // Find rocks in same row from result set
            var _rr = result.Where(r => r.row == rock.row).ToList();

            // Find cube rocks in the same row
            var _cr = cubeRocks.Where(r => r.row == rock.row).ToList();

            // Find lowest row from _rr and _cr between the current rock and the south of the map
            var collisions = _rr
                .Concat(_cr)
                .Where(r => r.col > rock.col)
                .OrderBy(r => r.col);

            if (collisions.Any())
            {
                var (col, row) = collisions.First();
                eastCol = col - 1;
            }

            // Add rock to result set with the new row
            result.Add((eastCol, rock.row));
        }
        return result;
    }

    public static List<Point> Cycle(this List<Point> roundRocks, List<Point> cubeRocks, int southIdx, int eastIdx)
    {
        return roundRocks
            .TiltNorth(cubeRocks)
            .TiltWest(cubeRocks)
            .TiltSouth(cubeRocks, southIdx)
            .TiltEast(cubeRocks, eastIdx);
    }
}