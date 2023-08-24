using AoCHelper;

namespace AdventOfCode.Day05;

public class Day05 : BaseDay
{
    private readonly string _input;

    public Day05() 
    {
        _input = File.ReadAllText($"{InputFilePath}"); 
    }

    public override ValueTask<string> Solve_1() =>
        new($"{ParseCrates(CrateMover9000)}");

    public override ValueTask<string> Solve_2() =>
        new($"{ParseCrates(CrateMover9001)}");

    private string ParseCrates(Action<Move> crane)
    {
        var stacks = MakeStacks();
        var moves = ParseRelocations(stacks);
        PopulateStacks(stacks, GetInitialCrateState());
        foreach (var move in moves)
        {
            crane(move);
        }
        return MakeResult(stacks);
    }
    
    private string MakeResult(List<Stack<char>> stacks) =>
        stacks.Aggregate("", (current, stack) => current + stack.Peek());

    private void CrateMover9000(Move move)
    {
        int count = move.Count;
        while (count-- > 0)
            move.Destination.Push(move.Source.Pop());
    }

    private void CrateMover9001(Move move)
    {
        var helper = new Stack<char>();
        CrateMover9000(move with{Destination = helper});
        CrateMover9000(move with{Source = helper});   
    }
    
    private List<Move> ParseRelocations(List<Stack<char>> stacks) => _input
            .Split($"{Environment.NewLine}{Environment.NewLine}")[1]
            .Split(Environment.NewLine)
            .Select(line => line.Split(' '))
            .Select(content => new Move(stacks[int.Parse(content[3]) - 1], int.Parse(content[1]), stacks[int.Parse(content[^1]) - 1]))
            .ToList();

    private List<Stack<char>> MakeStacks() => _input
            .Split($"{Environment.NewLine}{Environment.NewLine}")[0]
            .Split(Environment.NewLine)
            .Last()
            .Chunk(4)
            .Select(_ => new Stack<char>())
            .ToList();
    
    private List<List<char>> GetInitialCrateState() => _input
            .Split(Environment.NewLine)
            .TakeWhile(line => line != "")
            .Select(line => line.Chunk(4).Select(x => x[1]).ToList())
            .ToList();

    private void PopulateStacks(List<Stack<char>> stacks, List<List<char>> crates)
    {
        
        for (int i = crates.Count - 2; i >= 0; i--)
        {
            for (int j = 0; j < crates[0].Count; j++)
            {
                if(crates[i][j] != ' ')
                    stacks[j].Push(crates[i][j]);
            }
        }
    }

    private record struct Move(Stack<char> Source, int Count, Stack<char> Destination);
}
