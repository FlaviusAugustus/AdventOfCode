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

    public override ValueTask<string> Solve_2() =>
        new($"{Part2()}");

    private int Part1() => AllCalibrationLines()
        .Select(GetCalibrationValue)
        .Sum();

    private int? Part2() => AllCalibrationLines()
        .Select(GetCalibrationValueWithLetters)
        .Sum();
    
    private int GetCalibrationValue(string calibrationLine)
    {
        var first = calibrationLine.First(char.IsDigit);
        var last = calibrationLine.Last(char.IsDigit);
        
        var value = (first - '0') * 10 + (last - '0');
        return value;
    }

    private int? GetCalibrationValueWithLetters(string calibrationLine)
    {
        var calibrationValue = new CalibrationValue();
        
        for (var i = 0; i < calibrationLine.Length; i++)
        {
            if (char.IsDigit(calibrationLine[i]))
            {
                calibrationValue.Set(calibrationLine[i] - '0');
            }
            
            var possibleMatches = Nums.Keys.Where(number => calibrationLine[i..].StartsWith(number));
            foreach (var number in possibleMatches)
            {
                calibrationValue.Set(Nums[number]);
            }
        }
        return calibrationValue.Number;
    }
    
   private IEnumerable<string> AllCalibrationLines() =>
        _input.Split(Environment.NewLine);
}

internal struct CalibrationValue()
{
    private int? _first;
    private int? _last;

    public int? Number
    {
        get
        {
            _last ??= _first;
            return _first * 10 + _last;
        }
    }

    public void Set(int number)
    {
        _last = _first == null ? _last : number;
        _first ??= number;
    }
}