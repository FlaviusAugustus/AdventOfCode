using System.Text.RegularExpressions;

namespace AdventOfCode;
using AoCHelper;

public class Day15 : BaseDay
{
    private readonly string _input;

    public Day15()
    {
        _input = File.ReadAllText($"../../../{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{CalculateTakenSpaces(2000000)}");
    
    public override ValueTask<string> Solve_2() =>
        new($"{GetDistressSignalFrequency()}");

    private IEnumerable<Sensor> AllSensors(string input) => input
        .Split(Environment.NewLine)
        .Select(Sensor.Parse);

    private int CalculateTakenSpaces(int y)
    {
        var taken = new HashSet<int>();
        foreach (var sensor in AllSensors(_input))
        {
            taken.UnionWith(GetTakenSpacesAt(y, sensor));
        }
        return taken.Count;
    }

    private IEnumerable<int> GetTakenSpacesAt(int y, Sensor sensor)
    {
        if (sensor.Manhattan - Math.Abs(sensor.Position.Y - y) < 0)
            return Enumerable.Empty<int>();
        var xmax = sensor.Manhattan - Math.Abs(sensor.Position.Y - y) + sensor.Position.X;
        var xmin = -sensor.Manhattan + Math.Abs(sensor.Position.Y - y) + sensor.Position.X;
        return Enumerable.Range(xmin, Math.Abs(xmax - xmin));
    }

    private long GetDistressSignalFrequency()
    {
        var list = new List<Point>();
        foreach (var sensor in AllSensors(_input))
        {
            list.AddRange(GetPosibleDistressSignalPositions(sensor));
        }

        foreach (var pos in list)
        {
            var isOk = true;
            foreach (var sensor in AllSensors(_input))
            {
                if (Sensor.CalculateManhattan(sensor.Position, pos) <= sensor.Manhattan)
                {
                    isOk = false;
                    break;
                }
            }

            if (isOk && pos is { X: >= 0 and <= 4000000, Y: <= 4000000 and >= 0 })
            {
                long x = pos.X * 4000000; 
                return x + pos.Y;
            }
        }
        return -1;
    }

    private IEnumerable<Point> GetPosibleDistressSignalPositions(Sensor sensor)
    {
        var minY = sensor.Position.Y - sensor.Manhattan - 1;
        var maxY = sensor.Position.Y + sensor.Manhattan + 1;
        for (var i = minY; i <= maxY && i <= 4000000; i++)
        {
            var res = MaxMinSensor(i, sensor).ToList();
            
            if(res.Count != 0)
            {
                if (res[0] + 1 >= 0 && res[0] + 1 <= 4000000)
                {
                    yield return new(res[0] + 1, i);
                }
                if (res[^1] - 1 >= 0 && res[^1] - 1 <= 4000000)
                {
                    
                    yield return new(res[^1] - 1, i);
                }
            }
        }
        
    }
    
    private IEnumerable<int> MaxMinSensor(int y, Sensor sensor)
    {
        if (sensor.Manhattan - Math.Abs(sensor.Position.Y - y) < 0)
            yield break;
        var xmax = sensor.Manhattan - Math.Abs(sensor.Position.Y - y) + sensor.Position.X;
        var xmin = -sensor.Manhattan + Math.Abs(sensor.Position.Y - y) + sensor.Position.X;
        yield return xmax;
        yield return xmin;
    }
}

internal record Sensor(Point Beacon, Point Position, int Manhattan)
{
    public static Sensor Parse(string input)
    {
        var matchX = new Regex(@"(?<=x=)[^,]*").Matches(input);
        var matchY = new Regex(@"(?<=y=)[^( |:)]*").Matches(input);
        var beacon = new Point(int.Parse(matchX[1].Value), int.Parse(matchY[^1].Value));
        var sensor = new Point(int.Parse(matchX[0].Value), int.Parse(matchY[0].Value));
        var manhattan = CalculateManhattan(beacon, sensor);
        return new(beacon, sensor, manhattan);
    }

    public static int CalculateManhattan(Point p1, Point p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }
}