using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode._2023.Day01;

public class Day01 : BaseDay
{
    private readonly string _input;

    private Dictionary<string, int> Nums { get; } = new()
    {
        { "one", 1 },
        { "two",  2 },
        { "three",  3 },
        { "four",  4 },
        { "five",  5 },
        { "six",  6 },
        { "seven", 7 },
        { "eight",  8 },
        { "nine",  9 }
    };

    public Day01()
    { 
        _input = File.ReadAllText($"../../../2023/{InputFilePath}");
    }
    
    public override ValueTask<string> Solve_1() =>
        new($"{Part1()}");

    public int Part1() => AllCalibrationLines()
        .Select(GetCalibrationValue)
        .Sum();

    public int? Part2() => AllCalibrationLines()
        .Select(GetCalibrationValueWithLetters)
        .Sum();
    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");

    public int GetCalibrationValue(string calibrationLine)
    {
        var first = calibrationLine.First(char.IsDigit);
        var last = calibrationLine.Last(char.IsDigit);
        
        var value = (first - '0') * 10 + (last - '0');
        return value;
    }

    public int? GetCalibrationValueWithLetters(string calibrationLine)
    {
        int? first = null;
        int? last = null;

        for (var i = 0; i < calibrationLine.Length; i++)
        {
            if (char.IsDigit(calibrationLine[i]))
            {
                if (first is null)
                {
                    first = calibrationLine[i] - '0';
                }
                else
                {
                    last = calibrationLine[i] - '0';
                }
            }

            foreach (var number in Nums.Keys.Where(number => calibrationLine[i..].StartsWith(number)))
            {
                if (first is null)
                {
                    first = Nums[number];
                }
                else
                {
                    last = Nums[number];
                }
            }
        }

        if (last == null)
            last = first;
        return ((first) * 10 + (last));
    }
    public IEnumerable<string> AllCalibrationLines() =>
        _input.Split(Environment.NewLine);
}

internal struct CalibrationValue(int? first, int? Last)
{
    public void Set(int number)
    {
        Last = first == null ? Last : number;
        first ??= number;
    }
}