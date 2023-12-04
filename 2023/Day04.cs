using System.Collections.Concurrent;
using AoCHelper;

namespace AdventOfCode._2023;

public class Day04 : BaseDay
{
    private readonly string _input;
    
    public Day04() => _input = File.ReadAllText($"../../../2023/{InputFilePath}");

    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    private int Part1()
    {
        var sum = 0d;
        foreach (var line in _input.Split(Environment.NewLine))
        {
            var scratchCard = ScratchCard.Parse(line);
            if(scratchCard.PresentWinnings.Count != 0)
                sum += Math.Pow(2, scratchCard.PresentWinnings.Count - 1);
        }
        return (int)sum;
    }

    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");

    private int Part2()
    {
        var sum = 0;
        var scratchCards = _input
            .Split(Environment.NewLine)
            .Select(ScratchCard.Parse)
            .ToList();

        foreach (var card in scratchCards)
        {
            sum += FindAllScratchCards(0, card, scratchCards);
        } 

        return sum;
    }

    private int FindAllScratchCards(int sum, ScratchCard card,  List<ScratchCard> scratchCards)
    {
        return 0;
    }
}

internal record ScratchCard(int Id, List<int> PresentWinnings)
{
    public static ScratchCard Parse(string line)
    {
        var split = line.Split(":");
        var id = int.Parse(split[0].Split(" ")[^1]);

        var nums = split[1].Split("|");
        var present= nums[0].Split(" ")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(int.Parse)
            .ToList();

        var winning = nums[1].Split(" ")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(int.Parse)
            .ToList();

        var presentWinnings = Enumerable.Intersect(present, winning).ToList();
        
        return new ScratchCard(id, presentWinnings);
    }
}