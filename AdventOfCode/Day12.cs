using AoCHelper;

namespace AdventOfCode;

class Day12 : BaseDay
{
    private readonly string _input;
    private const char Start = 'S';
    private const char End = 'E';

    public Day12()
    {
        _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{Solve1()}");

    public override ValueTask<string> Solve_2() =>
        new($"{Solve2()}");

    private int Solve1()
    {
        var start = FindPosition(Start);
        var end = FindPosition(End);
        return new PathFinder(_input.Split(Environment.NewLine))
            .FindShortestPathLength(end)
            .Single(cell => cell.Position == start)
            .Distance;
    }

    private int Solve2()
    {
        var end = FindPosition(End);
        return new PathFinder(_input.Split(Environment.NewLine))
            .FindShortestPathLength(end)
            .Where(c => c.Value == 'a')
            .MinBy(cell => cell.Distance)! 
            .Distance;
    }

    private Point FindPosition(char symbol)
    {
        var splitInput = _input.Split(Environment.NewLine);
        var y = splitInput
            .Select((s, i) => new { Index = i, Value = s })
            .Single(item => item.Value.Contains(symbol)).Index;
        var x = splitInput[y].IndexOf(symbol);
        return new(x, y);
    }
}

internal class Cell
{
    private readonly char _value;
    public required char Value
    {
        get => _value;
        init => _value = value switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => value
        };
    }
    public required Point Position { get; init; } 
    public required int Distance { get; set; }

    public IEnumerable<Cell> GetNeighbours(Dictionary<Point, Cell> grid)
    {
        var points = new []
        {
            Position with { X = Position.X - 1 },
            Position with { Y = Position.Y + 1 },
            Position with { X = Position.X + 1 },
            Position with { Y = Position.Y - 1 }
        };
        foreach (var point in points)
        {
            if (grid.TryGetValue(point, out var cell))
                yield return cell;
        }
    }
}

internal class PathFinder
{
    private readonly string[] _grid;

    public PathFinder(string[] grid) =>
        _grid = grid;
    

    private IEnumerable<Cell> GetAllCells()
    {
        for (var i = 0; i < _grid.Length; i++)
        for (var j = 0; j < _grid[i].Length; j++) 
            yield return new Cell
            {
                Position = new (j, i), 
                Value = _grid[i][j], 
                Distance = int.MaxValue
            };
    }

    public IEnumerable<Cell> FindShortestPathLength(Point start)
    {
        Dictionary<Point, Cell> cel = GetAllCells().ToDictionary(cell => cell.Position);
        PriorityQueue<Cell, int> priority = new(Comparer<int>.Create((x, y) => x - y));
        cel[start].Distance = 0;
        priority.Enqueue(cel[start], cel[start].Distance);
        while (priority.Count != 0)
        {
            var current = priority.Dequeue();
            foreach (var neighbour in current.GetNeighbours(cel))
            {
                var alt = current.Distance + 1;
                if (alt < neighbour.Distance && Math.Abs(current.Value - neighbour.Value) <= 1 || current.Value < neighbour.Value)
                {
                    neighbour.Distance = alt;
                    priority.Enqueue(neighbour, neighbour.Distance);
                }
            }
        }
        return cel.Values;
    }
}