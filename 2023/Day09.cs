using AoCHelper;

namespace AdventOfCode._2023.Day09;

public class Day09 : BaseDay
{
   private readonly string _input;
   
   public Day09()
   {
      _input = File.ReadAllText($"../../../2023/{InputFilePath}");
   }

   public override ValueTask<string> Solve_1() =>
      new($"{Solve(ParseAllRanges())}");
   
   public override ValueTask<string> Solve_2() =>
      new($"{Solve(ParseAllRangesReverse())}");

   private int Solve(IEnumerable<List<int>> ranges) => ranges
      .Sum(range => range[^1] + ExtrapolateRange(range, range.Count));
   
   private int ExtrapolateRange(List<int> range, int n)
   {
      var sum = 0;
      if (range[..n].All(c => c == 0))
         return sum;
      var i = 0;
      for (; i < n - 1; i++)
      {
         range[i] = range[i + 1] - range[i];
      }

      sum += range[i - 1] + ExtrapolateRange(range, n - 1);
      return sum;
   }

   private IEnumerable<List<int>> ParseAllRanges() => _input
      .Split("\n")
      .Select(ParseRange);
   
   private IEnumerable<List<int>> ParseAllRangesReverse() => ParseAllRanges()
      .Select(r =>
         {
            r.Reverse();
            return r;
         });

   private List<int> ParseRange(string line) => line
      .Split(" ")
      .Select(int.Parse)
      .ToList();
}
