using AoCHelper;

namespace AdventOfCode._2023.Day05;

public class Day05 : BaseDay
{
    private readonly string _input; 
    
    public Day05() => _input = File.ReadAllText($"../../../2023/{InputFilePath}");
    
    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    public override ValueTask<string> Solve_2() =>
        new($"{null}");
    
    private long Part1()
    {
        var inputSplit = _input.Split($"{Environment.NewLine}{Environment.NewLine}");
        var seeds = ParseSeeds(inputSplit[0]).ToList();
        var maps = ParseMaps(inputSplit);
        return GetMinSeedValue(seeds, maps);
    }

    private long Part2()
    {
        var inputSplit = _input.Split($"{Environment.NewLine}{Environment.NewLine}");
        var seedRanges = ParseSeedsRange(inputSplit[0]).ToList();
        var maps = ParseMaps(inputSplit);
        var res = new List<Range>();
        foreach (var seedRange in seedRanges)
        {
            res.AddRange(ConvertSeedRange(seedRange, maps));
        }

        return res.MinBy(c => c.Start)!.Start;
    }

    private static long GetMinSeedValue(IList<long> seeds, IList<SeedMap> maps)
    {
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

    private IEnumerable<Range> ParseSeedsRange(string input)
    {
        return ParseSeeds(input)
            .Chunk(2)
            .Select(c => new Range(c[0], c[0] + c[1]));
    }

    private static IEnumerable<Range> ConvertSeedRange(Range seedRange, List<SeedMap> maps)
    {
        var res = new List<Range>(ConvertFromMap(maps[0], seedRange));
        var r2 = new List<Range>();
        foreach (var map in maps[1..])
        {
            r2.Clear();
            foreach (var newRange in res) 
            {
                r2.AddRange(ConvertFromMap(map, newRange));
            }
            res = r2;
        }
        return res;
    }

    private static IEnumerable<Range> ConvertFromMap(SeedMap map, Range seedRange)
    {
        foreach (var convertInfo in map.Map)
        {
            if (seedRange.End < convertInfo.SourceStart ||
                seedRange.Start > convertInfo.SourceStart + convertInfo.Range)
            {
                continue;
            }

            var start = Math.Max(convertInfo.SourceStart, seedRange.Start);
            var end = Math.Min(convertInfo.SourceStart + convertInfo.Range, seedRange.End);

            yield return new Range(convertInfo.Convert(start), convertInfo.Convert(end));

            if (start != seedRange.Start)
                yield return seedRange with { End = start };
            if (end != seedRange.End)
                yield return seedRange with { Start = end };
        }
    }
}

internal record Range(long Start, long End);

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


