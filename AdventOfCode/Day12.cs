using AdventOfCode;
using AoCHelper;

namespace AdventOfCode;

class Day12 : BaseDay
{
    private readonly string _input; 

    public Day12()
    {
        _input = File.ReadAllText($"../../../{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{new PathFinder().FindShortestPathLength(_input.Split(Environment.NewLine))}");

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}

internal class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Value  { get; set; }
    public int Distance { get; set; }

    public IEnumerable<Cell> GetNeighbours(List<List<Cell>> grid)
    {
        var deltas = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
        foreach (var delta in deltas)
        {
            if (IsValidPosition(X + delta[0], Y + delta[1], grid))
            {
                yield return grid[Y + delta[1]][X + delta[0]];
            }
        }
    }

    private static bool IsValidPosition(int x, int y, List<List<Cell>> grid) =>
        x >= 0 && y >= 0 && x < grid[0].Count && y < grid.Count;
}

internal class PathFinder
{
    private readonly PriorityQueue<Cell, int> _priority = new(Comparer<int>.Create((x, y) => x - y));
    
    public int FindShortestPathLength(string[] grid)
    {
        Cell end = new();
        List<List<Cell>> cells = new(grid.Length);
        for (var i = 0; i < grid.Length; i++)
        {
            List<Cell> row = new();
            for (var j = 0; j < grid[i].Length; j++)
            {
                var dist = 10000;
                if (grid[i][j] == 'S')
                    dist = 0;
                var cell = new Cell { X = j, Y = i, Value = grid[i][j], Distance = dist };
                if (grid[i][j] == 'S')
                {
                    cell.Value = 'a';
                    _priority.Enqueue(cell, cell.Distance);
                }
                if (grid[i][j] == 'E') {
                    cell.Value = 'z';
                    end = cell;
                }
                row.Add(cell);
            }
            cells.Add(row);
        }

        while (_priority.Count != 0)
        {
            var current = _priority.Dequeue();
            foreach (var neighbour in current.GetNeighbours(cells))
            {
                var alt = current.Distance + 1;
                if (alt < neighbour.Distance && (Math.Abs(current.Value - neighbour.Value) <= 1 || neighbour.Value < current.Value))
                {
                    neighbour.Distance = alt;
                    _priority.Enqueue(neighbour, alt);
                }
            }
        }

        return cells[end.Y][end.X].Distance;
    }
}