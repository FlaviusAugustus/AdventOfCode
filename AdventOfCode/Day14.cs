namespace AdventOfCode;
using AoCHelper;

public class Day14 : BaseDay
{
    private readonly string _input;

    public Day14()
    {
        _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{Solve1()}");

    public override ValueTask<string> Solve_2() =>
        new($"{Solve2()}");

    private int Solve2()
    {
        var cave = Cave.Parse(_input);
        var queue = new Queue<Coordinate>();
        var count = 0;
        queue.Enqueue(new Coordinate(500, 0));
        while (queue.Any())
        {
            var current = queue.Dequeue();
            if (!cave.Map.Contains(current))
            {
                count++;
                cave.Map.Add(current);
                foreach (var move in PossibleMoves(current))
                {
                    if (!cave.Map.Contains(move) && move.Y <= cave.Bottom + 1)
                    {
                        queue.Enqueue(move);
                    }
                }
            }
        }
        return count;
    }

    private int Solve1()
    {
        var count = 0;
        var cave = Cave.Parse(_input);
        while (true)
        {
            var sand = SimulateSand(cave, new Coordinate(500, 0));
            if (sand.Y == cave.Bottom)
            {
                return count;
            }
            count++;
            cave.Map.Add(sand);
        }
    }

    private Coordinate SimulateSand(Cave cave, Coordinate sand)
    {
        if (sand.Y == cave.Bottom)
            return sand;
        
        foreach (var move in PossibleMoves(sand))
        {
            if (!cave.Map.Contains(move))
            {
                sand = SimulateSand(cave, move);
                break;
            }
        }
        return sand;
    }
    
    private IEnumerable<Coordinate> PossibleMoves(Coordinate coordinate)
    {
        yield return coordinate with { Y = coordinate.Y + 1 }; 
        yield return new(coordinate.X - 1, coordinate.Y + 1);
        yield return new(coordinate.X + 1, coordinate.Y + 1);
    }
}

internal record struct Coordinate(int X, int Y)
{
    public static Coordinate Parse(string input)
    {
        var rawCoords = input.Split(",");
        var x = int.Parse(rawCoords[0]);
        var y = int.Parse(rawCoords[1]);
        return new(x, y);
    }

    public static int LineLength(Coordinate beg, Coordinate end)
    {
        var x = end.X - beg.X;
        var y = end.Y - beg.Y;
        return (int)Math.Sqrt(x * x + y * y);
    }

    public static Coordinate Direction(Coordinate beg, Coordinate end)
    {
        var x = end.X - beg.X;
        var y = end.Y - beg.Y;
        return new(Math.Sign(x), Math.Sign(y));
    }
}

internal class Cave
{
    public HashSet<Coordinate> Map { get; private init; } = new();
    public int Bottom { get; private init; }

    public static Cave Parse(string input)
    {
        var dict = input
            .Split(Environment.NewLine)
            .SelectMany(ParseLine)
            .ToHashSet();

        return new Cave { Map = dict, Bottom = dict.MaxBy(coordinate => coordinate.Y).Y };
    }

    private static IEnumerable<Coordinate> ParseLine(string line)
    {
        var coordinates = line.Split(" -> ");
        for(var i = 0; i < coordinates.Length - 1; i++)
        {
            var beg = Coordinate.Parse(coordinates[i]);
            var end = Coordinate.Parse(coordinates[i + 1]);
            var direction = Coordinate.Direction(beg, end);
            var current = beg;
            for (var j = 0; j <= Coordinate.LineLength(beg, end); j++)
            {
                yield return current;
                current.X += direction.X;
                current.Y += direction.Y;
            }
        }
    }
}