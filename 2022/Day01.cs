using AoCHelper;

namespace AdventOfCode._2022.Day01;

public class Day01 : BaseDay
{
   private readonly string _input;
   
   public Day01()
   {
      _input = File.ReadAllText($"../../../{InputFilePath}");
   }

   public override ValueTask<string> Solve_1() =>
      new($"{GetCaloriesPerElf().First()}");
   
   
   public override ValueTask<string> Solve_2() =>
      new($"{GetCaloriesPerElf().Take(3).Sum()}");

   private IEnumerable<int> GetCaloriesPerElf() => 
      from elf in _input.Split($"{Environment.NewLine}{Environment.NewLine}") 
      let calories = elf.Split(Environment.NewLine).Select(int.Parse).Sum() 
      orderby calories descending 
      select calories;
   
}
