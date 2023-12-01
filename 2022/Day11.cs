using AoCHelper;

namespace AdventOfCode._2022.Day11;

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

    private long Part1() =>
        ExecuteRounds(20, ParseMonkeys(), x => x / 3);

    private long Part2()
    {
        var monkeys = ParseMonkeys();
        var multipliedDivisors = monkeys
            .Select(monkey => monkey.Divisor)
            .Aggregate((current, next) => current*next);
        return ExecuteRounds(10000, monkeys, x => x % multipliedDivisors);
    }

    private List<Monkey> ParseMonkeys() => _input
        .Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(Monkey.Parse)
            .ToList();

    private long CalculateResult(IEnumerable<Monkey> monkeys) => monkeys
            .Select(monkey => monkey.ItemsHandled)
            .OrderBy(items => items)
            .Reverse()
            .Take(2)
            .Aggregate((first, last) => first*last);
    
    private long ExecuteRounds(int rounds, List<Monkey> monkeys, Func<long, long> worryManager)
    {
        while(rounds-- > 0)
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
                var item = monkey.Items.Dequeue();
                var inspected = worryManager(monkey.Operation(item));
                var id = inspected % monkey.Divisor == 0 ? monkey.TrueId : monkey.FalseId;

                monkey.ItemsHandled++;
                monkeys[id].Items.Enqueue(inspected);
            }
        }
    }
}

internal class Monkey {

    public required Queue<long> Items { get; init; }
    public required Func<long, long> Operation { get; init; }
    public required int Divisor { get; init; }
    public required int TrueId { get; init; }
    public required int FalseId { get; init; }
    public required long ItemsHandled { get; set; }

    public static Monkey Parse(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var items = lines[1].Split(":")[1].Split(",");
        var parsedItems = new Queue<long>(items.Select(long.Parse));

        var operatorString = lines[2].Split(" ")[^2];
        var operand = lines[2].Split(" ")[^1];
        var operation = ParseOperation(operatorString, operand);
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

    private static Func<long, long> ParseOperation(string operatorString, string operand) => operand switch
    {
        "old" => i => i * i,
        _ => ParseOperator(operatorString, operand)
    };
    
    private static Func<long, long> ParseOperator(string operatorString, string operand) => operatorString switch
    {
        "*" => i => i * int.Parse(operand),
        "+" => i => i + int.Parse(operand),
        _ => throw new ArgumentException(operatorString)
    };
}

