using AoCHelper;

namespace AdventOfCode._2023.Day03;

public class Day03 : BaseDay
{
    private readonly string _input;
    private readonly string[] _inputSplit;

    public Day03()
    {
        _input = File.ReadAllText($"../../../2023/{InputFilePath}");
        _inputSplit = _input.Split(Environment.NewLine);
    }

    public override ValueTask<string> Solve_1() =>
        new($"{SumEngineParts()}");

    public override ValueTask<string> Solve_2() =>
        new($"{SumGearRatios()}");

    private int SumGearRatios() =>
        AllEngineNumbers()
            .Select(SetPartCoordinateForGears)
            .GroupBy(e => e.PartCoord)
            .Where(g => g.Count() == 2)
            .Sum(g => g.First().ToInt(_inputSplit)*g.Last().ToInt(_inputSplit));
    
    private int SumEngineParts() => 
        AllEngineNumbers()
            .Where(IsConnected)
            .Sum(engineNumber => engineNumber.ToInt(_inputSplit));

    private IEnumerable<EngineNumber> AllEngineNumbers() => 
        AllCoordinates()
            .Where(c => char.IsDigit(_inputSplit[c.Y][c.X]))
            .Select(c => new EngineNumber(
                c, 
                c with { X = ExtractNumber(c) },
                new Point(-1, -1)));

    private int ExtractNumber(Point coordinate)
    {
        var j = coordinate.X;
        while (j != _inputSplit[0].Length && char.IsDigit(_inputSplit[coordinate.Y][j]))
        {
            j++;
        }
        return j;
    }

    private IEnumerable<Point> AllCoordinates() 
    {
        for (var i = 0; i < _inputSplit.Length; i++)
        {
            for (var j = 0; j < _inputSplit[0].Length; j++)
            {
                yield return new Point(j, i);
            }
        }
    }
    
    private bool IsConnected(EngineNumber engineNumber) =>
        GetSurroundingCoordinates(engineNumber).Any(c => EngineNumber.IsPart(c, _inputSplit));

    private EngineNumber SetPartCoordinateForGears(EngineNumber engineNumber)
    {
        var coordinate = GetSurroundingCoordinates(engineNumber)
            .Where(c => _inputSplit[c.Y][c.X] == '*').ToList();
        if (coordinate.Count == 0)
            return engineNumber with{ PartCoord = EngineNumber.IncorrectPartCoord };
        return engineNumber with { PartCoord = coordinate.Single() };
    }

    private IEnumerable<Point> GetSurroundingCoordinates(EngineNumber engineNumber)
    {
        var possibleCoords = new List<Point>
        {
            engineNumber.Start with { X = engineNumber.Start.X - 1 },
            engineNumber.End 
        };
        for (var i = engineNumber.Start.X - 1; i < engineNumber.End.X + 1; i++)
        {
            possibleCoords.Add(new Point(i, engineNumber.Start.Y - 1));
            possibleCoords.Add(new Point(i, engineNumber.Start.Y + 1));
        }

        return possibleCoords
            .Where(coord => coord.IsValid(_inputSplit));
    }
}

internal record EngineNumber(Point Start, Point End, Point PartCoord)
{
    public static readonly Point IncorrectPartCoord = new Point(-1, -1);
    
    public int ToInt(string[] chars) =>
        int.Parse(chars[Start.Y][Start.X..End.X]);

    public static bool IsPart(Point point, string[] chars) =>
        chars[point.Y][point.X] != '.' && !char.IsDigit(chars[point.Y][point.X]);
}

internal record Point(int X, int Y)
{
    public bool IsValid(string[] chars) =>
        X >= 0 && X < chars[0].Length && Y >= 0 && Y < chars.Length;
}