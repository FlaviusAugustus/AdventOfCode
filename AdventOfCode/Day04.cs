using AoCHelper;

namespace AdventOfCode.Day04;

public class Day04 : BaseDay
{
    private readonly string _input;
    
    public Day04()
    {
       _input = File.ReadAllText($"{InputFilePath}"); 
    }
    public override ValueTask<string> Solve_1() =>
        new($"{CalculateOverlaps(CalculateOverlap)}");
    
    public override ValueTask<string> Solve_2() =>
        new($"{CalculateOverlaps(CalculateOverlapAssignmentCount)}");
    
    private int CalculateOverlaps(Func<string, int> evaluator) => _input
        .Split(Environment.NewLine)
        .Select(evaluator)
        .Sum();
    
    private int CalculateOverlapAssignmentCount(string line) =>
        Calculate(MakeRange(line.Split(',')[0]), MakeRange(line.Split(',')[1]));

    private int Calculate(Range range1, Range range2) =>
        !(range1.End.Value < range2.Start.Value || range1.Start.Value > range2.End.Value) ? 1 : 0;

    private int CalculateOverlap(string line) =>
        DoesOverlap(MakeRange(line.Split(',')[0]), MakeRange(line.Split(',')[1])) ? 1 : 0;
        
    private Range MakeRange(string line)
    {
        var itemArray = line.Split('-');
        if (itemArray.Length == 2)
            return new(int.Parse(itemArray[0]), int.Parse(itemArray[1]));
        throw new ArgumentException();
    }

    private bool DoesOverlap(Range range1, Range range2) =>
        range1.Start.Value <= range2.Start.Value && range1.End.Value >= range2.End.Value ||
        range2.Start.Value <= range1.Start.Value && range2.End.Value >= range1.End.Value;
}

