namespace AoC.Tasks.Year2022.Day01;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input)
    {
        _list = InputHelper.ToStringList(input);
    }

    public object PartOne() => MostCalories(_list);
    public object PartTwo() => MostThreeCalories(_list);

    private static int MostCalories(List<string> input)
    {
        var sums = SummarizeCalories(input);

        return sums.Max();
    }

    private static IEnumerable<int> SummarizeCalories(List<string> input)
    {
        var lists = CalorieChunks(input);
        var sums = lists.Select(x =>
            x.Select(int.Parse).ToList().Sum()
        );
        
        return sums;
    }

    private static IEnumerable<List<string>> CalorieChunks(List<string> input)
    {
        var lists = new List<List<string>>();

        while (input.Any())
        {
            var temp = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (temp.Any())
            {
                lists.Add(temp);
            }

            input = input.Skip(int.Max(temp.Count, 1)).ToList();
        }

        return lists;
    }

    private static int MostThreeCalories(List<string> input)
    {
        var sums = SummarizeCalories(input);

        return sums.OrderDescending().Take(3).Sum();
    }
}
