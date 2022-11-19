using AoC.Helpers;

namespace AoC.Tasks.Year2022.Day01;

public class Solution : ISolver
{
    private readonly List<int> _list;

    public Solution(string input)
    {
        _list = InputHelper.ToIntList(input).ToList();
    }

    public object PartOne() => MeasurementIncreaseCount(_list);
    public object? PartTwo() => DoStuff2();

    private static int MeasurementIncreaseCount(IReadOnlyCollection<int> input)
    {
        var count = input.Zip(input.Skip(1)).Count(x => x.First < x.Second);
        return count;
    }

    private static int DoStuff2()
    {
        return int.MaxValue;
    }
}
