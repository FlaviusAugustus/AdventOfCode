using AoCHelper;

namespace AdventOfCode._2023;

public class Day06 : BaseDay
{
    private readonly string _input;
    
    public Day06()
    { 
        _input = File.ReadAllText($"../../../2023/{InputFilePath}");
    }
    
    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");

    private int Part2() =>
        GetPossibleScores(ParseRacePart2());

    private int Part1() => ParseRaces()
        .Select(GetPossibleScores)
        .Aggregate((mul, curr) => mul * curr);

    private static int GetPossibleScores(Race race)
    {
        var delta = race.Time * race.Time - 4 * race.Distance;
        var deltaRoot = Math.Sqrt(delta);
        var x1 = Math.Ceiling((-race.Time - deltaRoot) / -2);
        var x2 = Math.Floor((-race.Time + deltaRoot) / -2);
        return (int)Math.Abs(x1 - x2) - 1;

    }
    
    private IEnumerable<Race> ParseRaces()
    {
        var inputSplit = _input.Split(Environment.NewLine);
        var timeRaw = inputSplit[0].Split(":")[1].Split(" ");
        var distanceRaw = inputSplit[1].Split(":")[1].Split(" ");

        var time = ParseArray(timeRaw);
        var distance = ParseArray(distanceRaw);

        return time.Zip(distance, (t, d) => new Race(t, d));
    }

    private Race ParseRacePart2()
    {
        
        var inputSplit = _input.Split(Environment.NewLine);
        var timeRaw = inputSplit[0].Split(":")[1].Trim();
        var timeJoined = new string(timeRaw.Where(c => c != ' ').ToArray());
        
        var distanceRaw = inputSplit[1].Split(":")[1].Trim();
        var distanceJoined = new string(distanceRaw.Where(c => c != ' ').ToArray());

        return new Race(long.Parse(timeJoined), long.Parse(distanceJoined));
    }
    
    private IEnumerable<int> ParseArray(string[] arrayRaw) => arrayRaw
        .Where(c => !string.IsNullOrWhiteSpace(c))
        .Select(int.Parse);
    
}

internal record Race(long Time, long Distance);