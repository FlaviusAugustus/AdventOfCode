using AoCHelper;

namespace AdventOfCode._2023.Day10;

public class Day10 : BaseDay
{
   private readonly string _input;
   
   public Day10()
   {
      _input = File.ReadAllText($"../../../{InputFilePath}");
   }

   public override ValueTask<string> Solve_1() =>
      new($"{null}");
   
   public override ValueTask<string> Solve_2() =>
      new($"{null}");

   private IEnumerable<Pipe> ParsePipes()
   {
      var inputSplit = _input.Split("\n");
      for (var y = 0; y < inputSplit.Length; y++)
      {
         for (var x = 0; x < inputSplit[0].Length; x++)
         {
            if (inputSplit[y][x] != '.' && inputSplit[y][x] != 'S')
               yield return Pipe.Parse(x, y, inputSplit[y][x]);
         }
      }
   }
   
}

internal record Pipe(int X, int Y, Directions PossibleDirections)
{
   public static Pipe Parse(int x, int y, char c) =>
      new Pipe(x, y, ParseDirection(c));
   
   private static Directions ParseDirection(char c) => c switch
   {
      '|' => Directions.North | Directions.South,
      '-' => Directions.East | Directions.West,
      'L' => Directions.North | Directions.East,
      'J' => Directions.North | Directions.West,
      '7' => Directions.South | Directions.West,
      'F' => Directions.South | Directions.East,
      '.' => 0,
      _ => throw new ArgumentException()
   };
}

[Flags]
internal enum Directions
{
   North = 1,
   East = 2,
   West = 4,
   South = 8
}