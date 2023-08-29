using AoCHelper;

class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1()  =>
        new($"{ExecuteRounds(20, WorryLevelPart1)}");

    public override ValueTask<string> Solve_2()  =>
        new($"{ExecuteRounds(10000, WorryLevelPart2)}");

    private long WorryLevelPart1(long worryLevel) =>
        worryLevel / 3;

    private long WorryLevelPart2(long worryLevel) =>
        worryLevel % 9699690; 

    public List<Monkey> ParseMonkeys() => _input
            .Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(monkeyRaw => Monkey.Parse(monkeyRaw))
            .ToList();

    private long CalculateResult(IEnumerable<Monkey> monkeys) => monkeys
            .Select(monkey => monkey.ItemsHandled)
            .OrderBy(items => items)
            .Reverse()
            .Take(2)
            .Aggregate((first, last) => first*last);
    
    public long ExecuteRounds(int rounds, Func<long, long> worryManager)
    {
        var monkeys = ParseMonkeys();
        foreach(var round in Enumerable.Range(1, rounds))
        {
            ExecuteRound(monkeys, worryManager);
        }
        return CalculateResult(monkeys);
    }

    private void ExecuteRound(List<Monkey> monkeys, Func<long, long> worryManager)
    {
        foreach(var monkey in monkeys)
        {
            foreach(var change in monkey.GetMoves(worryManager)) 
            {
                monkeys[change.Id].Items.Add(change.Item);
            }
            monkey.ExecuteRound();
        }
    }
}

record Move(int Id, long Item);

class Monkey {

    public required List<long> Items {get; set;}
    public required Func<long, long> Operation{get; set;}
    public required int Divisor{get; set;}
    public required int TrueId{get; set;}
    public required int FalseId{get; set;}
    public required long ItemsHandled{get; set;}

    public IEnumerable<Move> GetMoves(Func<long, long> worryManager)
    {
        foreach(var item in Items) 
        {
            var inspected = worryManager(Operation(item));
            yield return new(inspected % Divisor == 0 ? TrueId : FalseId, inspected);
        }
    }
    
    public void ExecuteRound()
    {
        ItemsHandled += Items.Count;
        Items.Clear();
    }

    public static Monkey Parse(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var items = lines[1].Split(":")[1].Split(",");
        var parsedItems = items.Select(i => long.Parse(i)).ToList();

        var o = lines[2].Split(" ")[^2];
        var operand = lines[2].Split(" ")[^1];
        Func<long, long> operation;
        if(operand == "old") 
        {
            operation = (i) => i * i;
        }
        else 
        {
            operation = o switch
            {
                "*"  => (i) => i * int.Parse(operand),
                "+" => (i) => i + int.Parse(operand),
                _ => throw new ArgumentException()
            };
        }
        
        var divisor = int.Parse(lines[3].Split(" ")[^1]);

        var trueId = int.Parse(lines[4].Split(" ")[^1]);
        var falseId = int.Parse(lines[5].Split(" ")[^1]);
        
        return new Monkey
        {
            Items = parsedItems,
            Operation = operation,
            Divisor = divisor,
            TrueId = trueId,
            FalseId = falseId,
            ItemsHandled = 0
        };
    }
}

