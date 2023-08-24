using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Net.Mail;
using AoCHelper;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace AdventOfCode.Day07;

public class Day07 : BaseDay
{
    private readonly string _input;
    
    public Day07()
    {
        _input = File.ReadAllText($"{InputFilePath}"); 
    }

    public override ValueTask<string> Solve_1() =>
        new($"{GetDirectorySizeSum(new FileSystem(_input))}");

    public override ValueTask<string> Solve_2() =>
        new($"{SmallestDirectoryToDelete(new FileSystem(_input))}");

    private int SmallestDirectoryToDelete(FileSystem fileSystem)
    {
        var minimumSize = 30_000_000 - (70_000_000 - fileSystem._directories["/"].Size);
        var result = fileSystem.Dirs
            .Where(dir => dir.Size > minimumSize)
            .MinBy(dir => dir.Size).Size;
        return result;
    }

    private int GetDirectorySizeSum(FileSystem fileSystem) => fileSystem.Dirs
        .Where(dir => dir.Size <= 100_000)
        .Sum(dir => dir.Size);
}

internal class FileSystem
{
    private readonly string _files;
    public readonly Dictionary<string, Directory> _directories = new();
    public IReadOnlyCollection<Directory> Dirs => new ReadOnlyCollection<Directory>(_directories.Values.ToArray());
    private readonly Stack<string> _path = new();
    private Directory _current;

    public FileSystem(string files)
    {
        _files = files;
        Build();
    }

    private void Build()
    { 
        BuildDirectoryHierarchy();
        CalculateDirectorySizes("/");
    }
    
    private int CalculateDirectorySizes(string directoryPath)
    {
        var subdirectoriesSize = 0;
        var directory = _directories[directoryPath];
        foreach (var subDirectory in directory.SubDirectories)
        {
            subdirectoriesSize += CalculateDirectorySizes(subDirectory);
        }
        _directories[directoryPath] = directory with { Size = directory.Size + subdirectoriesSize };
        return directory.Size + subdirectoriesSize;
    }

    private void BuildDirectoryHierarchy()
    {
        foreach (var line in _files.Split(Environment.NewLine))
        {
            var splitLine = line.Split(' ');
            if (IsCommand(splitLine))
                ChangeDirectory(splitLine[2]);
            else
                ParseFile(splitLine);
        }
        _directories.Add(_current.Name, _current);
    }

    private bool IsCommand(string[] line) =>
        line[0] == "$" && line[1] == "cd";

    private void ParseFile(string[] line)
    {
        if (line[0] == "dir")
            _current.SubDirectories.Add(line[1] + ' ' + string.Join(' ', _path));
        if (int.TryParse(line[0], out int size))
            _current.Size += size;
    }
        
    private void ChangeDirectory(string arg)
    {
        if (arg == "..")
        {
            _path.Pop();
            return;
        }
        if (_current != default)
            _directories.Add(_current.Name, _current);
        _path.Push(arg);
        _current = new Directory(string.Join(' ', _path), 0, new List<string>());
    }
    
    public record struct Directory(string Name, int Size, List<string> SubDirectories);
}
