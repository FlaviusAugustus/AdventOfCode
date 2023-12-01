using AoCHelper;

namespace AdventOfCode._2022.Day03;

public class Day03 : BaseDay
{
    private readonly string _input;
    public Day03()
    {
      _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{SumWeights()}");

    public override ValueTask<string> Solve_2() =>
        new($"{SumGroupWeights(MakeGroup())}");

    private int SumWeights() => 
        _input.Split(Environment.NewLine)
        .Select(CalculateWeight)
        .Sum();

    private int CalculateWeight(string rucksack)
    {
        var items = DivideRucksack(rucksack);
        foreach (var item in items.Item1)
        {
            if (items.Item2.Contains(item))
                return CharToWeight(item);
        }
        throw new ArgumentException();
    }

    private IEnumerable<string[]> MakeGroup() => _input.Split(Environment.NewLine)
        .Select((value, index) => new {Value = value, Index = index})
        .GroupBy(x => x.Index / 3)
        .Select(x => x.Select(x => x.Value).ToArray());
    
    private int SumGroupWeights(IEnumerable<string[]> arr)
    {
        IEnumerable<int> weights =
            from rucksacks in arr
            select FindGroupWeight(rucksacks);
        return weights.Sum();
    }

    private int FindGroupWeight(string[] group)
    {
        foreach (var item in group[0])
        {
            if (group[1].Contains(item) && group[2].Contains(item))
                return CharToWeight(item);
        }
        throw new ArgumentException();
    }

    private (string, string) DivideRucksack(string rucksack) =>
        (rucksack[..(rucksack.Length / 2)], rucksack[(rucksack.Length / 2)..]);

    private int CharToWeight(char item) => item switch
    {
        _ when char.IsUpper(item) => item - 'A' + 27,
        _ when char.IsLower(item) => item - 'a' + 1,
        _ => throw new ArgumentException()
    };

}
