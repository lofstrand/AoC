using System.Text.RegularExpressions;

namespace AoC.Tasks.Year2023.Day03;

public class Solution(string input) : ISolver
{
    private readonly List<string> _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        var numbers = _list.Select((x, index) => Regex.Matches(x, @"\d+")
                .Select(match => new Part(match.Value, index, match.Index)))
            .SelectMany(x => x);
        var symbols = _list.Select((x, index) => Regex.Matches(x, @"[^0-9.]")
                .Select(match => new Part(match.Value, index, match.Index)))
            .SelectMany(x => x);

        var result = numbers.Where(x => symbols.Any(symbol => IsAdjacent(x, symbol)))
            .Select(x => int.Parse(x.Value))
            .Sum();

        return result;
    }


    public object PartTwo()
    {
        var numbers = _list.Select((x, index) => Regex.Matches(x, @"\d+")
                .Select(match => new Part(match.Value, index, match.Index)))
            .SelectMany(x => x);
        var gears = _list.Select((x, index) => Regex.Matches(x, @"\*")
                .Select(match => new Part(match.Value, index, match.Index)))
            .SelectMany(x => x);

        var result = gears.Select(gear =>
            {
                return numbers.Where(number => IsAdjacent(gear, number)).ToList();
            })
            .Where(partList => partList.Count == 2)
            .Select(partList => partList.Select(part => int.Parse(part.Value)).Aggregate((a, b) => a * b))
            .Sum();
        
        return result;
    }

    private static bool IsAdjacent(Part partA, Part partB)
    {
        if (Math.Abs(partA.Row - partB.Row) > 1)
        {
            return false;
        }

        if (partA.Col > partB.Col + partB.Value.Length)
        {
            return false;
        }

        if (partB.Col > partA.Col + partA.Value.Length)
        {
            return false;
        }

        return true;
    }
}

public record Part(string Value, int Row, int Col);
