using System.Collections.Immutable;
using AoCHelper;

namespace AdventOfCode._2023.Day07;

public class Day07 : BaseDay
{
    private readonly string _input;
    
    public Day07()
    { 
        _input = File.ReadAllText($"../../../2023/{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{CalculateBets(new HandBasicComparer())}");

    public override ValueTask<string> Solve_2() =>
        new($"{CalculateBets(new HandAdvancedComparer())}");

    private long CalculateBets(IComparer<Hand> comparer) => _input
        .Split(Environment.NewLine)
        .Select(Hand.Parse)
        .OrderByDescending(c => c, comparer)
        .Select((h, i) => h.Bet * (i + 1))
        .Sum();
}

internal class HandAdvancedComparer : HandBasicComparer
{
    protected override int GetCardValue(Hand card)
    {
        var chars = card.HandString
            .GroupBy(c => c)
            .Select(g => new { Letter = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();
        
        var cardValueBase = chars[0].Count;
        if (cardValueBase == 5)
            return (int)HandType.FiveOfKind;
        var jokerCount = card.HandString.Count(c => c == 'J');
        if (jokerCount > 0 && chars[0].Letter != 'J')
            cardValueBase += jokerCount;
        else if (jokerCount > 0)
            cardValueBase = chars[1].Count + jokerCount;
        if (cardValueBase == 5)
            return (int)HandType.FiveOfKind;
        return GetHandTypeValue(cardValueBase, chars[1].Count);
    }

    protected override int GetCharValue(char c) =>
        c == 'J' ? -1 : base.GetCharValue(c);
}

internal class HandBasicComparer : IComparer<Hand>
{
    public int Compare(Hand? left, Hand? right)
    {
        if (left == right)
            return 0;
        if (left is not null && right is not null)
            return CompareHands(left, right);
        throw new ArgumentException("Hands can't be null");
    }

    private int CompareHands(Hand left, Hand right)
    {
        var leftValue = GetCardValue(left);
        var rightValue = GetCardValue(right);
        if (rightValue == leftValue)
            return CompareStrings(left, right);
        
        return leftValue > rightValue ? -1 : 1;
    }

    private int CompareStrings(Hand left, Hand right)
    {
        for (var i = 0; i < 5; i++)
        {
            var leftCharValue = GetCharValue(left.HandString[i]);
            var rightCharValue = GetCharValue(right.HandString[i]);
            if (leftCharValue != rightCharValue)
                return leftCharValue > rightCharValue ? -1 : 1;
        }
        throw new ArgumentException("Incorrect input");
    }

    protected virtual int GetCharValue(char c) => c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        <= '9' and >= '2' => c - '2',
        _ => throw new ArgumentException("Card doesnt exist")
    };

    protected virtual int GetCardValue(Hand card)
    {
        var chars = card.HandString
            .GroupBy(c => c)
            .Select(g => new { Letter = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        var highestCount = chars[0].Count;
        if (highestCount == 5)
            return (int)HandType.FiveOfKind;
        var secondHighestCount = chars[1].Count;
        return GetHandTypeValue(highestCount, secondHighestCount);
    } 
    
    protected int GetHandTypeValue(int highestCount, int secondHighestCount) => highestCount switch
        {
            2 when secondHighestCount == 2 => (int)HandType.TwoPair,
            3 when secondHighestCount == 2 => (int)HandType.FullHouse,
            4 => (int)HandType.FourOfKind,
            2 => (int)HandType.OnePair,
            3 => (int)HandType.ThreeOfKind,
            1 => (int)HandType.HighCard,
            _ => (int)HandType.FiveOfKind
        };
}

internal record Hand(string HandString, int Bet) 
{
    public static Hand Parse(string line)
    {
        var lineSplit = line.Split(" ");
        var hand = lineSplit[0];
        var bet = int.Parse(lineSplit[1]);

        return new Hand(hand, bet);
    }
}

internal enum HandType
{
    HighCard = 1,
    OnePair,
    TwoPair,
    ThreeOfKind,
    FullHouse,
    FourOfKind,
    FiveOfKind
}
