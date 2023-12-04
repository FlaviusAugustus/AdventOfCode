using System.Reflection;
using AoCHelper;

var yearArgument = args[0];
if (!int.TryParse(yearArgument, out var year))
{
    Console.WriteLine("Argument must be a year");
    return 0;
}


var daysToSolve = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => 
        t is { Namespace: not null, IsClass: true } && 
        t.Namespace.StartsWith($"AdventOfCode._{yearArgument}") &&
        t.BaseType == typeof(BaseDay)) 
    .ToList();

if (daysToSolve.Count == 0)
{
    Console.WriteLine($"AoC for year {yearArgument} hasn't been implemented.");
    return 0;
}
    
await Solver.Solve(daysToSolve);
return 0;



