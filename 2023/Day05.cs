using AoCHelper;

namespace AdventOfCode._2023;

public class Day05 : BaseDay
{
    private readonly string _input; 
    
    public Day05() => _input = File.ReadAllText($"../../../2023/{InputFilePath}");
    
    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    private long Part1()
    {
        var inputSplit = _input.Split($"{Environment.NewLine}{Environment.NewLine}");
        var seeds = ParseSeeds(inputSplit[0]).ToList();
        var maps = ParseMaps(inputSplit);
        var res = new List<long>();

        for (var i = 0; i < seeds.Count; i++)
        {
            foreach (var map in maps)
            {
                var convert = map.Map.Where(t => t.IsInRange(seeds[i])).ToList();
                if (convert.Count != 0)
                {
                    seeds[i] = convert[0].Convert(seeds[i]);
                }
            }
        }

        return seeds.Min();
    }

    private static List<SeedMap> ParseMaps(string[] mapsRaw) => mapsRaw
        .Skip(1)
        .Select(s => s.Split(Environment.NewLine))
        .Select(SeedMap.Parse)
        .ToList();
    
    private IEnumerable<long> ParseSeeds(string input)
    {
        var seedsRaw = input.Split(':')[1].Split(" ");
        return seedsRaw
            .Where(c => !string.IsNullOrEmpty(c))
            .Select(long.Parse);
    }


public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}

internal record SeedConvertInfo(long DestStart, long SourceStart, long Range)
{
    public static SeedConvertInfo Parse(string line)
    {
        var lineSplit = line.Split(" ");
        var destRangeStart = long.Parse(lineSplit[0]);
        var sourceRangeStart = long.Parse(lineSplit[1]);
        var range = long.Parse(lineSplit[2]);

        return new SeedConvertInfo(destRangeStart, sourceRangeStart, range);
    }

    public bool IsInRange(long seed) =>
        seed >= SourceStart && seed <= SourceStart + Range;

    public long Convert(long seed) =>
        Math.Abs(seed - SourceStart) + DestStart;
}

internal record SeedMap(List<SeedConvertInfo> Map)
{
    public static SeedMap Parse(string[] lines)
    {
        var significant = lines[1..];
        var map = significant
            .Select(SeedConvertInfo.Parse)
            .ToList();
        
        return new SeedMap(map);
    }
}


