
using AoCHelper;

namespace AdventOfCode._2023;

public class Day02 : BaseDay
{
    private readonly string _input;
    
    public Day02() => _input = File.ReadAllText($"../../../2023/{InputFilePath}");

    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");

    private int Part1() => _input
        .Split(Environment.NewLine)
        .Select(Game.Parse)
        .Where(g => g.Throws.All(t => t.HasEnoughCubes()))
        .Sum(g => g.Id);

    private int Part2() => _input
        .Split(Environment.NewLine)
        .Select(Game.Parse)
        .Select(g => g.GetMinimumRequiredCubes())
        .Sum(t => t.Red * t.Green * t.Blue);
}

internal record Throw(int Red, int Green, int Blue)
{
    public bool HasEnoughCubes() =>
        Blue <= 14 && Red <= 12 && Green <= 13;
    
    public static Throw ParseThrow(string line)
    {
        var red = 0;
        var green = 0;
        var blue = 0;
        var lineSplit = line.Split(" ");
        for (var i = 0; i < lineSplit.Length - 1; i += 2)
        {
            var count = int.Parse(lineSplit[i]);
            var colour = lineSplit[i + 1];
            switch (colour)
            {
                case "red," or "red":
                    red = count;
                    break;
                case "green," or "green":
                    green = count;
                    break;
                case "blue," or "blue":
                    blue = count;
                    break;
            }
        }
        return new Throw(red, green, blue); 
    }
}
internal record Game(int Id, List<Throw> Throws)
{
    public static Game Parse(string line)
    {
        var lineSplit = line.Split(' ');
        var id = int.Parse(lineSplit[1][..^1]);
        
        var index = line.IndexOf(':') + 1;
        var lineGame = line[index..].Split(';');
        var throws = lineGame.Select(game => Throw.ParseThrow(game.Trim())).ToList();

        return new Game(id, throws);
    }

    public Throw GetMinimumRequiredCubes()
    {
        var green = Throws.Max(g => g.Green);
        var red = Throws.Max(g => g.Red);
        var blue = Throws.Max(g => g.Blue);
        return new Throw(red, green, blue);
    }
}