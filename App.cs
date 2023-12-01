using System.Data;
using System.Reflection;
using AdventOfCode._2022;
using AdventOfCode.Utils;
using AoCHelper;

var yearArgument = args[0];
if (!int.TryParse(yearArgument, out var year))
{
    Console.WriteLine("Argument must be a year"); 
}

Console.WriteLine(yearArgument);

var types = Assembly.GetEntryAssembly()!.GetTypes();

var solvers = Assembly.GetEntryAssembly()!
    .GetTypes()
    .Where(t => t.GetInterfaces().Contains(typeof(IYearSolver))) 
    .OrderBy(t => t.Name);

var solverType = solvers.Single(s => s.Name.Contains(yearArgument));
var solverInstance = Activator.CreateInstance(solverType);

solverType.InvokeMember(nameof(IYearSolver.Solve), 
     BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
       null, solverInstance, null);


