using System.Collections.Concurrent;
using System.Security.Cryptography;
using AoCHelper;

namespace AdventOfCode._2023;

public class Day04 : BaseDay
{
    private readonly string _input;
    
    public Day04() => _input = File.ReadAllText($"../../../2023/{InputFilePath}");

    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");
    
    private int Part1() => _input.Split(Environment.NewLine)
        .Select(ScratchCard.Parse)
        .Where(c => c.WinningCount > 0)
        .Select(c => (int)Math.Pow(2, c.WinningCount-1))
        .Sum();

    private int Part2()
    {
        var scratchCards = _input
            .Split(Environment.NewLine)
            .Select(ScratchCard.Parse)
            .ToList();

        var sum = scratchCards.Count;
        sum += scratchCards.Sum(card => FindAllScratchCards(card, scratchCards));
        
        return sum;
    }

    private static int FindAllScratchCards(ScratchCard card,  IReadOnlyList<ScratchCard> scratchCards)
    {
        var sum = 0;
        if (card.WinningCount == 0)
        {
            return 0;
        }

        var cardsWon = Enumerable.Range(card.Id + 1, card.WinningCount).ToList();
        foreach (var cardWon in cardsWon)
        {
            sum += FindAllScratchCards(scratchCards[cardWon - 1], scratchCards);
        }

        sum += cardsWon.Count;
        return sum;
    }
}

internal record ScratchCard(int Id, int WinningCount)
{
    public static ScratchCard Parse(string line)
    {
        var split = line.Split(":");
        var id = int.Parse(split[0].Split(" ")[^1]);

        var nums = split[1].Split("|");
        var present = ParseNums(nums[0]);

        var winning = ParseNums(nums[1]);

        var presentWinnings = Enumerable.Intersect(present, winning).Count();
        
        return new ScratchCard(id, presentWinnings);
    }
    
    private static IEnumerable<int> ParseNums(string line) => line.Split(" ")
        .Where(s => !string.IsNullOrEmpty(s))
        .Select(int.Parse)
        .ToList();
}