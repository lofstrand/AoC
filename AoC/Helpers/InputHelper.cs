using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Helpers;

public static class InputHelper
{
    public static async Task GetInput(string year, string day)
    {
        var cookie = Environment.GetEnvironmentVariable("SESSION_COOKIE");
        var projectRootPath = PathHelper.GetProjectRootPath();
        var filePath = Path.Combine(projectRootPath, "Tasks", $"Year{year}", $"Day{day}", "input.txt");
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(filePath))
        {
            var uri = new Uri("https://adventofcode.com");
            var cookies = new CookieContainer();
            cookies.Add(uri, new Cookie("session", cookie));
            await using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            using var client = new HttpClient(handler);
            client.BaseAddress = uri;
            using var response = await client.GetAsync($"/{year}/day/{day}/input");
            await using var stream = await response.Content.ReadAsStreamAsync();

            await stream.CopyToAsync(file);
            // Create Solution.cs file if it does not exist
            var solutionFilePath = Path.Combine(projectRootPath, $"Tasks/Year{year}/Day{day}/Solution.cs");
            if (!File.Exists(solutionFilePath))
            {
                CreateSolutionFile(solutionFilePath, year, day);
            }
        }
    }

    private static void CreateSolutionFile(string filePath, string year, string day)
    {
        var content = $$"""
                        namespace AoC.Tasks.Year{{year}}.Day{{day}};
    
                        public class Solution : ISolver
                        {
                            private readonly List<string> _list;
                            
                            public Solution(string input)
                            {
                                _list = InputHelper.ToStringList(input);
                            }
        
                            public object PartOne()
                            {
                                return int.MinValue;
                            }
    
                            public object PartTwo()
                            {
                                return int.MinValue;
                            }
                        }
                        """;
        File.WriteAllText(filePath, content, Encoding.UTF8);
    }

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

        var list = Regex.Split(input, @"\n+").Where(s => !string.IsNullOrEmpty(s)).ToList();

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

    public static IEnumerable<Tuple<string, string>> ToTupleList(this IEnumerable<string> input, string separator = " ")
    {
        return input
            .Select(line => line
                .Split(separator))
            .Select(splitVal => new Tuple<string, string>(splitVal[0], splitVal[1]))
            .ToList();
    }

    public static List<List<T>> ChunkList<T>(this IEnumerable<T> data, int size)
    {
        return data
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / size)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static IEnumerable<Tuple<List<int>, List<int>>> ToTupleRange(this IEnumerable<string> input, string separator = ",")
    {
        var result = input.Select(line => line.Split(separator))
            .Select(splitVal =>
            {
                var (p1, p2) = (splitVal[0].Split("-"), splitVal[1].Split("-"));
                var p1Start = int.Parse(p1[0]);
                var p1Count = int.Parse(p1[1]) - p1Start + 1;
                var p2Start = int.Parse(p2[0]);
                var p2Count = int.Parse(p2[1]) - p2Start + 1;

                return new Tuple<List<int>, List<int>>(Enumerable.Range(p1Start, p1Count).ToList(), Enumerable.Range(p2Start, p2Count).ToList());
            })
            .ToList();
        
        // 54412 too low

        return result;
    }
}
