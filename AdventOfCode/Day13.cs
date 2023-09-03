using System.Text.RegularExpressions;
using AoCHelper;

class Day13 : BaseDay
{
    private readonly string _input;

    public Day13()
    {
        _input = File.ReadAllText($"../../../{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{PrintElements()}");

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

    private string PrintElements()
    {
        var packetLeft = AllPackets().First().Left[1..];
        var packetRight = AllPackets().First().Right[1..];
        var itemsLeft = GetAllElements(packetLeft, new Dictionary<string, List<string>>());
        var itemsRight = GetAllElements(packetRight, new Dictionary<string, List<string>>());
        var indexLeft = 0;
        var indexRight = 0;
        while (indexLeft < itemsLeft.Count && indexRight < itemsRight.Count)
        {
             
        }

        return "";
    }

    private List<object> GetAllElements(string packet, Dictionary<string, List<string>> dict)
    {
        var item = "";
        var rank = 0;
        var items = new List<object>();
        foreach (var character in packet)
        {
            item += character;

            if (character == '[')
            {
                rank++;
            }

            if (character == ']')
            {
                rank--;
            }

            if (rank == 0)
            {
                if (item != ",")
                {
                    items.Add(item);
                }
                if (item[0] == '[')
                {
                    items.AddRange(GetAllElements(item[1..], dict));
                }

                item = "";
            }
        }
        
        return items;
    }
}

internal record Packet(string Left, string Right)
{
    public static Packet Parse(string line)
    {
        var split = line.Split(Environment.NewLine);
        var left = split[0];
        var right = split[1];
        return new(left, right);
    }
}