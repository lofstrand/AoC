namespace AoC.Tasks.Year2022.Day06;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        return MessageMarker(4);
    }

    public object PartTwo()
    {
        return MessageMarker(14);
    }

    private int MessageMarker(int distinctCount)
    {
        var chArr = _list.First().ToCharArray().ToList();
        chArr.Reverse();
        var stack = new Stack<char>(chArr);
        var buffer = new List<char>();

        int processCount = 0;
        while (stack.Any())
        {
            if (buffer.Distinct().Count() == distinctCount)
            {
                break;
            }

            buffer.Add(stack.Pop());
            if (buffer.Count > distinctCount)
            {
                buffer.RemoveAt(0);
            }

            processCount++;
        }

        return processCount;
    }
}
