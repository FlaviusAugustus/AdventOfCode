using System.Diagnostics;
using AoCHelper;

namespace AdventOfCode.Day02;

public class Day02 : BaseDay
{
   private readonly string _input;
   
   public Day02()
   {
      _input = File.ReadAllText($"{InputFilePath}");
   }

   public override ValueTask<string> Solve_1() =>
      new($"{SumPoints(EvalRound)}");

   public override ValueTask<string> Solve_2() =>
      new($"{SumPoints(EvalRoundWithStrategy)}");
   
   private int SumPoints(Func<string, int> evaluator)
   {
       IEnumerable<int> points =
            from round in _input.Split(Environment.NewLine)
            select evaluator(round);
            return points.Sum();
   }

   private int EvalRoundWithStrategy(string round) =>
      EvalRound(CalculateStrategy(round));

   private string CalculateStrategy(string round)
   {
      var strategy = GetMove(round[^1]);
      var opponent = round[0];
      return $"{opponent} {GetStrategy(strategy, GetMove(opponent)).ToChar()}";
   }

   private Move GetStrategy(Move strategy, Move opponent) => strategy switch
   {
      Move.Rock => opponent.Next().Next(),
      Move.Paper => opponent,
      Move.Scissors => opponent.Next(),
      _ => throw new ArgumentException()
   };

   private int EvalRound(string round) =>
      GetPointsFromMove(GetMove(round[0]), GetMove(round[^1]));

   private Move GetMove(char move) => move switch
   {
      'A' or 'X' => Move.Rock,
      'B' or 'Y' => Move.Paper,
      'C' or 'Z' => Move.Scissors,
      _ => throw new ArgumentException($"{move}")
   };
   
   private int GetPointsFromMove(Move opponent, Move player) =>
         GetRoundPoints(opponent, player) + GetHandPoints(player);

   private int GetRoundPoints(Move opponent, Move player) => player switch
   {
      _ when opponent.Next() == player => 6,
      _ when opponent == player => 3,
      _ => 0
   };

   private int GetHandPoints(Move player) =>
      (int)player + 1;
}

   enum Move { Rock, Paper, Scissors }

   static class EnumExtension
   {
      public static Move Next(this Move move) => move switch
      {
         Move.Rock => Move.Paper,
         Move.Paper => Move.Scissors,
         Move.Scissors => Move.Rock,
         _ => throw new ArgumentException()
     };

      public static char ToChar(this Move move) => move switch
      {
         Move.Rock => 'X',
         Move.Paper => 'Y',
         Move.Scissors => 'Z',
         _ => throw new ArgumentException()
      };
   }
