using AoCHelper;

namespace AdventOfCode._2022.Day06;

public class Day06 : BaseDay
{

    private readonly string _input;

    public Day06()
    {
        _input = File.ReadAllText($"{InputFilePath}"); 
    }

    public override ValueTask<string> Solve_1() =>
        new($"{GetEndOfPacketMarker(4)}");

    public override ValueTask<string> Solve_2() =>
        new($"{GetEndOfPacketMarker(14)}");
        
    private int GetEndOfPacketMarker(int length) => StringChunks(length)
        .First(chunk => IsPacketStartMarker(chunk.chunk))
        .index;
    
    private IEnumerable<Chunk> StringChunks(int length)
    { 
        for (int start = 0, end = length; end < _input.Length; start++, end++)
            yield return new(_input[start..end], end);
    }

    private bool IsPacketStartMarker(string chunk) => chunk
        .Select((c, index) => new{Current = index, Last = chunk.LastIndexOf(c)})
        .All(c => c.Current == c.Last);

    private record struct Chunk(string chunk, int index);
}
