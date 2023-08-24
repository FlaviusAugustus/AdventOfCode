using AoCHelper;
using System.Text;

class Day10 : BaseDay
{

    private readonly string _input;
    public Day10()
    {
        _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{SumCycles(new int[] { 20, 60, 100, 140, 180, 220 })}");

    public override ValueTask<string> Solve_2() =>
        new($"{Render()}");

    private int SumCycles(int[] cycles) => AllSignalStrengths()
            .Select((signal, index) => new { Value = signal * (index + 1), Index = (index + 1) })
            .Where(p => cycles.Contains(p.Index))
            .Sum(p => p.Value);
        
    private string Render()
    {
        var cycle = 1;
        var builder = new StringBuilder();
        foreach (var signal in AllSignalStrengths())
        {
            if (signal == cycle % 40 || signal + 1 == cycle % 40 || signal + 2 == cycle % 40)
                builder.Append("#");
            else
                builder.Append(".");
            if (cycle % 40 == 0)
                builder.Append(Environment.NewLine);
            cycle++;
        }
        return builder.ToString();
    }

    private IEnumerable<int> AllSignalStrengths()
    {
        var result = 1;
        foreach (var operation in AllOperations())
        {
            for (int i = 0; i < operation.Length; i++)
            {
                yield return result;
            }
            result += operation.Value;
        }
    }

    private IEnumerable<Operation> AllOperations() => _input
        .Split(Environment.NewLine)
        .Select(line => Operation.Parse(line));
}

internal record Operation(int Value, int Length)
{
    public static Operation Parse(string line)
    {
        var lineSplit = line.Split(' ');
        var length = lineSplit.Length != 1 ? int.Parse(lineSplit[1]) : 0;

        return new Operation
        (
            length,
            GetOperationLength(lineSplit[0])
        );
    }

    private static int GetOperationLength(string operation) => operation switch
    {
        "addx" => 2,
        "noop" => 1,
        "" => 0,
        _ => throw new ArgumentException(operation)
    };
}

