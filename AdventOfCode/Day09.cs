using AoCHelper;

namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText($"{InputFilePath}");
    }

    public override ValueTask<string> Solve_1() =>
        new($"{CountUniqueTailPositions(2)}");

    public override ValueTask<string> Solve_2() =>
		new($"{CountUniqueTailPositions(10)}");

    private int CountUniqueTailPositions(int length)
    {
        var positions = new HashSet<Point>();
		var rope = Enumerable.Range(0, length).Select(p => new Point(0, 0)).ToList();

		foreach(var move in AllMoves()) 
		{
            for (var i = 0; i < move.Length; i++)
            {
				positions.Add(rope[^1]);
            	MoveRope(rope, move.Direction);
            }
        }
		positions.Add(rope[^1]);
        return positions.Count;
    }

	private void MoveRope(List<Point> rope, Direction direction) 
	{
		rope[0] = new(rope[0].X + direction.X, rope[0].Y + direction.Y);
		for(var i = 1; i < rope.Count; i++) 
		{
			var dx = rope[i - 1].X - rope[i].X;
			var dy = rope[i - 1].Y - rope[i].Y;
			if(!AreOneApart(rope[i - 1], rope[i]))
			{
				rope[i] = new Point(rope[i].X + Math.Sign(dx) ,rope[i].Y + Math.Sign(dy));
            }
		}
	}

	private IEnumerable<Move> AllMoves() 
	{
		foreach (var line in _input.Split(Environment.NewLine))
		{
			yield return Move.Parse(line);
		}
	}

    private bool AreOneApart(Point head, Point tail) =>
        Math.Abs(head.X - tail.X) == 1 && Math.Abs(head.Y - tail.Y) == 0 ||
        Math.Abs(head.Y - tail.Y) == 1 && Math.Abs(head.X - tail.X) == 0 ||
        Math.Abs(head.X - tail.X) == 1 && Math.Abs(head.Y - tail.Y) == 1;
}

internal record Point(int X, int Y);

internal record Move(int Length, Direction Direction)
{
    public static Move Parse(string line)
    {
        if(line == String.Empty)
            return new(0 ,new(0, 0));
        var lineSplit = line.Split(" ");
        var length = int.Parse(lineSplit[1]);
        var direction = Direction.ParseDirection(lineSplit[0]);
        return new(length, direction);
    }
}

internal record Direction(int X, int Y)
{
    public static Direction ParseDirection(string direction) => direction switch
    {
        "U" => new(0, 1),
        "D" => new(0, -1),
        "L" => new(-1, 0),
        "R" => new(1, 0),
        _ => throw new ArgumentException(direction)
    };
}
