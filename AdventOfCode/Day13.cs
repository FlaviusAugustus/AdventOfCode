using Newtonsoft.Json;
using AoCHelper;
using Newtonsoft.Json.Linq;

class Day13 : BaseDay
{
    private readonly string _input;

    public Day13()
    {
        _input = File.ReadAllText($"../../../{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{1}");

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

    private int Solve1() => AllPackets()
        .Select((packet, i) => new { Packet = packet, Index = i })
        .Where(p => IsInCorrectOrder(p.Packet))
        .Sum(p => p.Index + 1);

    private IEnumerable<Packet> AllPackets() => _input
        .Split($"{Environment.NewLine}{Environment.NewLine}")
        .Select(Packet.Parse);

    private bool IsInCorrectOrder(Packet packet)
    {
        throw new NotImplementedException();
    }

}

internal record Packet(JArray left, JArray right)
{
    public static Packet Parse(string line)
    {
        var split = line.Split(Environment.NewLine);
        var left = split[0];
        var right = split[1];
        return new(JArray.Parse(left), JArray.Parse(right));
    }
}