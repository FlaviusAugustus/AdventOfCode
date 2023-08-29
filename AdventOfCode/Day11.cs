using AoCHelper;

class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1()  =>
        new($"{Part1()}");

    public override ValueTask<string> Solve_2()  =>
        new($"{Part2()}");

    public long Part1() =>
        ExecuteRounds(20, ParseMonkeys(), x => x / 3);

    public long Part2()
    {
        var monkeys = ParseMonkeys();
        var multipliedDivisors = monkeys.Select(monkey => monkey.Divisor).Aggregate((current, next) => current*next);
        return ExecuteRounds(10000, monkeys, x => x % multipliedDivisors);
    }

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
    
    public long ExecuteRounds(int rounds, List<Monkey> monkeys, Func<long, long> worryManager)
    {
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
            while(monkey.Items.Any())
            {
                monkey.ItemsHandled++;

                var item = monkey.Items.Dequeue();
                var inspected = worryManager(monkey.Operation(item));

                var id = inspected % monkey.Divisor == 0 ? 
                    monkey.TrueId : 
                    monkey.FalseId;

                monkeys[id].Items.Enqueue(inspected);
            }
        }
    }
}


class Monkey {

    public required Queue<long> Items {get; set;}
    public required Func<long, long> Operation{get; set;}
    public required int Divisor{get; set;}
    public required int TrueId{get; set;}
    public required int FalseId{get; set;}
    public required long ItemsHandled{get; set;}

    public static Monkey Parse(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var items = lines[1].Split(":")[1].Split(",");
        var parsedItems = new Queue<long>(items.Select(i => long.Parse(i)));

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

