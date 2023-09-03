namespace AdventOfCode;
using AoCHelper;

public class Day14 : BaseDay
{
    private readonly string _input;

    public Day14()
    {
        _input = File.ReadAllText($"../../../{InputFilePath}");
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
                cave.Map.Add(current);
                count++;
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
            var sand = new Coordinate(500, 0);
            while (true)
            {
                var next = PossibleMoves(sand)
                    .Where(coord => !cave.Map.Contains(coord))
                    .ToList();
                if (sand.Y > cave.Bottom)
                {
                    return count;
                }
                if (!next.Any())
                {
                    break;
                }
                sand = next.FirstOrDefault();
            }
            cave.Map.Add(sand);
            count++;
        }

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
    
    
}

internal class Cave
{
    public HashSet<Coordinate> Map { get; init; } = new();
    public int Bottom { get; init; }

    public static Cave Parse(string input)
    {
        var dict = new HashSet<Coordinate>();
        foreach (var line in input.Split(Environment.NewLine))
        {
            var coordinates = line.Split(" -> ");
            for(int i = 0; i < coordinates.Length - 1; i++)
            {
                var beg = Coordinate.Parse(coordinates[i]);
                var end = Coordinate.Parse(coordinates[i + 1]);
                var x = end.X - beg.X;
                var y = end.Y - beg.Y;
                var diff = x == 0 ? y : x;
                var c = beg;
                for (int j = 0; j < Math.Abs(diff); j++)
                {
                    if (x != 0)
                        c.X += Math.Sign(diff);
                    else
                        c.Y += Math.Sign(diff);
                    dict.Add(c);
                }

                dict.Add(beg);
                dict.Add(end);

            }
        }
        var bottom = dict.MaxBy(coordinate => coordinate.Y)!.Y;

        return new Cave { Map = dict, Bottom = bottom };
    }

}