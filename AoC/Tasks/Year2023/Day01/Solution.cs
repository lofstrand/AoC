using System.Text.RegularExpressions;

namespace AoC.Tasks.Year2023.Day01;

public class Solution(string input) : ISolver
{
    private readonly List<string> _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        var sum = _list.Select(x =>
        {
            var firstA = Regex.Match(x, @"\d");
            var lastA = Regex.Match(x, @"\d", RegexOptions.RightToLeft);

            var first = int.Parse(firstA.Value);
            var last = int.Parse(lastA.Value);
            return first * 10 + last;
        }).Sum();
        
        return sum;
    }

    public object PartTwo()
    {
        var dict = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
            { "0", 0 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 }
        };
        var pattern = string.Join("|", dict.Keys.Select(Regex.Escape));

        var sum = _list.Select(x =>
        {
            var firstA = Regex.Match(x, pattern);
            var lastA = Regex.Match(x, pattern, RegexOptions.RightToLeft);

            dict.TryGetValue(firstA.Value, out var first);
            dict.TryGetValue(lastA.Value, out var last);
            return first * 10 + last;
        }).Sum();

        return sum;
    }
}
