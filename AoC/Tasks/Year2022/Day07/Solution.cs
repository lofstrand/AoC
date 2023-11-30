namespace AoC.Tasks.Year2022.Day07;

public class Solution : ISolver
{
    private readonly List<string> _list;
    private readonly Folder _root = new() { Name = "/" };

    public Solution(string input)
    {
        _list = InputHelper.ToStringList(input);
        MapInputToFoldersAndFiles(_root);
    }

    public object PartOne()
    {
        var sum = FlattenFolders(_root)
            .Where(x => x.Size <= 100000)
            .Select(x => x.Size)
            .Sum();
        return sum;
    }

    public object PartTwo()
    {
        const int diskSpace = 70000000;
        const int spaceNeeded = 30000000;
        var totalUsedSpace = _root.Size;
        var freeSpace = diskSpace - totalUsedSpace;

        var size = FlattenFolders(_root)
            .Where(x => x.Size >= spaceNeeded - freeSpace)
            .OrderBy(x => x.Size)
            .First()
            .Size;
        return size;
    }

    private IEnumerable<Folder> FlattenFolders(Folder folder)
    {
        var folders = new Stack<Folder>();
        folders.Push(folder);

        while (folders.Count > 0)
        {
            var f = folders.Pop();
            yield return f;

            f.Folders.ForEach(x => folders.Push(x));
        }
    }

    private void MapInputToFoldersAndFiles(Folder rootFolder)
    {
        var currentDir = rootFolder;
        _list.Reverse();
        var stack = new Stack<string>(_list);

        while (stack.Any())
        {
            var instruction = stack.Pop();
            if (instruction == "$ ls")
            {
                while (stack.Any() && !stack.Peek().StartsWith("$"))
                {
                    var fileOrFolder = stack.Pop();
                    if (fileOrFolder.StartsWith("dir"))
                    {
                        var folder = new Folder { Name = fileOrFolder.Split(new[] { "dir " }, StringSplitOptions.None)[1] };
                        currentDir.UpsertFolder(folder);
                    }
                    else
                    {
                        var size = int.Parse(fileOrFolder.Split(" ")[0]);
                        var name = fileOrFolder.Split(" ")[1];
                        var file = new File() { Size = size, Name = name };
                        currentDir.UpsertFile(file);
                    }
                }
            }

            if (instruction.StartsWith("$ cd "))
            {
                var folderName = instruction.Split(new[] { "$ cd " }, StringSplitOptions.None)[1];
                currentDir = folderName switch
                {
                    "/" => rootFolder,
                    ".." => currentDir.Parent ??= rootFolder,
                    _ => currentDir.Folders.First(x => x.Name.Equals(folderName))
                };
            }
        }
    }
}

public class Folder
{
    public string Name { get; set; } = string.Empty;
    public Folder? Parent { get; set; }
    public List<Folder> Folders { get; } = new();
    public List<File> Files { get; } = new();

    public int Size
    {
        get
        {
            var sum = 0;
            Files.ForEach(x => sum += x.Size);
            Folders.ForEach(x => sum += x.Size);
            return sum;
        }
    }

    public void UpsertFolder(Folder folder)
    {
        if (Folders.Exists(x => x.Name.Equals(folder.Name)))
        {
            return;
        }

        folder.Parent = this;
        Folders.Add(folder);
    }

    public void UpsertFile(File file)
    {
        if (Files.Exists(x => x.Name.Equals(file.Name)))
        {
            return;
        }

        Files.Add(file);
    }
}

public class File
{
    public int Size { get; set; }
    public string Name { get; set; } = string.Empty;
}
