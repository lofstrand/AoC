using AoC.Helpers;

namespace AoC.Tasks.Year2022.Day04;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        var tuples = _list.ToTupleRange();
        var result = tuples.Where(tuple =>
            {
                var (r1, r2) = (tuple.Item1, tuple.Item2);
                return !r1.Except(r2).Any() || !r2.Except(r1).Any();
            })
            .Count();

        return result;
    }

    public object? PartTwo()
    {
        var tuples = _list.ToTupleRange();
        var result = tuples.Where(tuple =>
            {
                var (r1, r2) = (tuple.Item1, tuple.Item2);
                return r1.Except(r2).Count() != r1.Count || r2.Except(r1).Count() != r2.Count;
            })
            .Count();

        return result;
    }
}
