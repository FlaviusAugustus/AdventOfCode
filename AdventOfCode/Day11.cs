using AoCHelper;

class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()  =>
        new($"{ExecuteRounds(20)}");

    public override ValueTask<string> Solve_2()  =>
        new($"");

    public List<Monkey> ParseMonkeys() => _input
            .Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(monkeyRaw => Monkey.Parse(monkeyRaw))
            .ToList();
    
    public int ExecuteRounds(int rounds)
    {
        var monkeys = ParseMonkeys();
        
        for(int i = 0; i < rounds; i++ )
        {
            for(int j = 0; j < monkeys.Count; j++)
            {
                for(int k = 0; k < monkeys[j].Items.Count; k++) 
                {
                    monkeys[j] = monkeys[j] with {itemsHandled = monkeys[j].itemsHandled+1};
                    var item = monkeys[j].Items[k];
                    if(monkeys[j].Operation(item) % monkeys[j].Divisor == 0)
                    {
                        monkeys[j].Items.Remove(item);
                        monkeys[monkeys[j].TrueId].Items.Add(item);
                    }
                    else {
                        monkeys[j].Items.Remove(item);
                        monkeys[monkeys[j].FalseId].Items.Add(item);
                    }
                }
            }
        }

        var res = monkeys.OrderBy(monkey => monkey.itemsHandled).Reverse().Take(2).ToList();
        return res[0].itemsHandled * res[1].itemsHandled;
    }

}

record Monkey
(
    int Id,
    List<int> Items,
    Func<int, int> Operation,
    int Divisor,
    int TrueId,
    int FalseId,
    int itemsHandled
) 
{
    public static Monkey Parse(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var id = int.Parse(lines[0].Split(" ")[^1][..^1]);

        var items = lines[1].Split(":")[1].Split(",");
        var parsedItems = items.Select(i => int.Parse(i)).ToList();

        var o = lines[2].Split(" ")[^2];
        var operand = int.Parse(lines[2].Split(" ")[^1]);
        Func<int, int> operation = o switch
        {
            "*"  => (i) => i * operand,
            "+" => (i) => i + operand,
            _ => throw new ArgumentException()
        };
        
        var divisor = int.Parse(lines[3].Split(" ")[^1]);

        var trueId = int.Parse(lines[4].Split(" ")[^1]);
        var falseId = int.Parse(lines[5].Split(" ")[^1]);
        
        return new(id, parsedItems, operation, divisor, trueId, falseId, 0);
    }

    
}

