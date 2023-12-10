using AoCHelper;

namespace AdventOfCode._2023;

public class Day08 : BaseDay
{
    private readonly string _input;
    
    public Day08() => _input = File.ReadAllText($"../../../2023/{InputFilePath}");

    public override ValueTask<string> Solve_1() =>
        new($"{Part1("AAA", (s) => s != "ZZZ")}");

    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");

    private int Part1(string start, Func<string, bool> endCondition)
    {
        var map = GetDirectionMap();
        var directions = _input.Split("\n\n")[0];
        var i = 0;
        var j = 0;
        var current = start;
        while (endCondition(current))
        {
            if (j == directions.Length)
            {
                j = 0;
            }
            if (directions[j] == 'L')
                current = map[current].Left;
            if (directions[j] == 'R')
                current = map[current].Right;
            
            i++;
            j++;
        }
        return i;
    }

    private long Part2()
    {
        var map = GetDirectionMap();
        var currentNodes = map.Keys
            .Where(k => k[^1] == 'A')
            .ToList();
        
        var roadLengths = currentNodes.Select(r => 0).ToList();

        for(var k = 0; k < currentNodes.Count; k++)
        {
            roadLengths[k] = Part1(currentNodes[k], s => s[^1] != 'Z');
        }

        return Lcm(roadLengths);
    }

    private long Lcm(IList<int> toCalc)
    {
        long ans = toCalc[0];
        for(var i = 1; i < toCalc.Count; i++)
        {
            ans = toCalc[i] * ans / Gcd(toCalc[i], ans);
        }

        return ans;
    }

    private long Gcd(long a, long b) =>
        b == 0 ? a : Gcd(b, a % b);
    
    private Dictionary<string, Directions> GetDirectionMap()
    {
        var directionsMap = new Dictionary<string, Directions>();
        var inputSplit = _input.Split("\n\n");
        
        foreach (var line in inputSplit[1].Split("\n"))
        {
            var lineSplit = line.Split(" = ");
            directionsMap[lineSplit[0]] = Directions.Parse(lineSplit[1]);
        }

        return directionsMap;
    }
    
}

internal record Directions(string Left, string Right)
{
    public static Directions Parse(string directionsRaw)
    {
        var directionsSplit = directionsRaw.Split(", ");
        var left = directionsSplit[0][1..];
        var right = directionsSplit[1][..^1];
        
        return new Directions(left, right);
    }
}