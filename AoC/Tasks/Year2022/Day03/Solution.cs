using System.Text.RegularExpressions;

namespace AoC.Tasks.Year2022.Day03;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne() => PrioritiesSum(_list);

    public object PartTwo() => GroupOfThree();

    private int GroupOfThree()
    {
        var chunkList = _list.ChunkList(3);
        var result = chunkList.Select(chunk =>
            {
                var first = chunk.ElementAt(0).ToCharArray().ToList();
                return first.First(ch => chunk.ElementAt(1).Contains(ch) && chunk.ElementAt(2).Contains(ch));
            })
            .Select(ConvertCharToInt)
            .Sum();

        return result;
    }

    private static int PrioritiesSum(IEnumerable<string> list)
    {
        var result = list
            .Select(x => new Tuple<string, string>(x.Substring(0, x.Length / 2), x.Substring(x.Length / 2, x.Length / 2)))
            .Select(x =>
            {
                var chArr = x.Item1.ToCharArray().ToList();
                return chArr.First(ch => x.Item2.Contains(ch.ToString()));
            })
            .Select(ConvertCharToInt)
            .Sum();

        return result;
    }

    private static int ConvertCharToInt(char x)
    {
        int val = 0;
        string xAsStr = x.ToString();
        if (Regex.IsMatch(xAsStr, "[a-z]"))
        {
            val = x - 96;
        }

        if (Regex.IsMatch(xAsStr, "[A-Z]"))
        {
            val = x - 38;
        }

        return val;
    }
}
