namespace AoC.Helpers;

public static class InputHelper
{
    public static string ReadTaskInput(string year, string day)
    {
        var projectRootPath = PathHelper.GetProjectRootPath();

        var taskInputPath = Path.Combine(projectRootPath, $"Tasks/Year{year}/Day{day}/input.txt");
        if (!File.Exists(taskInputPath))
        {
            throw new FileNotFoundException($"Task input with year {year} and day {day} does not exists.");
        }

        var input = GetNormalizedInput(taskInputPath);
        return input;
    }

    private static string GetNormalizedInput(string file)
    {
        var input = File.ReadAllText(file);
        if (input.EndsWith("\n"))
        {
            input = input[..^1];
        }

        return input;
    }

    public static List<string> ToStringList(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be empty or null");
        }
        
        var list = input.Split(Environment.NewLine)
            .ToList();

        return list;
    }

    public static IEnumerable<int> ToIntList(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be empty or null");
        }

        var list = input.Split(Environment.NewLine)
            .Select(int.Parse)
            .ToList();
        return list;
    }
}
