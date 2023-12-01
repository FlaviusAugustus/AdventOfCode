using AoCHelper;

namespace AdventOfCode._2022.Day08;

public class Day08 : BaseDay
{
    private readonly string[] _input;

    private static Direction Left = new(-1, 0);
    private static Direction Rigth = new(1, 0);
    private static Direction Up = new(0, 1);
    private static Direction Down = new(0, -1);
    
    public Day08()
    {
        _input = File.ReadAllText($"{InputFilePath}").Split(Environment.NewLine); 
    }

    public override ValueTask<string> Solve_1() =>
        new($"{CountVisible()}");

    public override ValueTask<string> Solve_2() =>
        new($"{GetMaxScenicScore()}");

    private IEnumerable<Tree> AllCoordinates()
    {
        for (var y = 0; y < _input.Length; y++)
        {
            for (var x = 0; x < _input[0].Length; x++)
            {
                yield return new Tree(_input[y][x], new(x, y));
            }
        }
    }

    private IEnumerable<Tree> TreesInDirection(Tree tree, Direction dir)
    {
        var x = tree.Position.X + dir.X;
        var y = tree.Position.Y + dir.Y;
        while (x < _input[0].Length && x >= 0 && y < _input.Length && y >= 0)
        {
            yield return new Tree(_input[y][x], new(x, y));
            x += dir.X;
            y += dir.Y;
        }
    }

    private bool IsVisibleInDirection(Tree tree, Direction dir) =>
        TreesInDirection(tree, dir).All(other => other.Height < tree.Height);

    private int CountVisible() => AllCoordinates()
        .Count(tree =>
            IsVisibleInDirection(tree, Left) || IsVisibleInDirection(tree, Rigth) || 
            IsVisibleInDirection(tree, Down) || IsVisibleInDirection(tree, Up));

    private int GetMaxScenicScore() => AllCoordinates()
        .Max(tree => 
            CountTreesInDirection(tree, Left) * CountTreesInDirection(tree, Rigth) * 
            CountTreesInDirection(tree, Down) * CountTreesInDirection(tree, Up));

    private int CountTreesInDirection(Tree tree, Direction dir)
    {
        var result = TreesInDirection(tree, dir).TakeWhile(other => other.Height < tree.Height).Count();
        return IsVisibleInDirection(tree, dir) ? result : result + 1;
    }
}
internal record struct Point(int X, int Y);
internal record Direction(int X, int Y);
internal record Tree(int Height, Point Position);
