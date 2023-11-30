using System.Text.RegularExpressions;

namespace AoC.Tasks.Year2022.Day05;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        var stacks = CratesToStacks();
        List<Instruction> instructions = GetInstructions();
        instructions.ForEach(instruction =>
        {
            for (int i = 0; i < instruction.Count; i++)
            {
                var val = stacks[instruction.From].Pop();
                stacks[instruction.To].Push(val);
            }
        });
        var resp = stacks.Where(x => x.Any()).Select(x => x.Pop()).Aggregate((i, j) => i + j);
        return resp;
    }

    public object PartTwo()
    {
        var stacks = CratesToStacks();
        List<Instruction> instructions = GetInstructions();
        instructions.ForEach(instruction =>
        {
            var tempStack = new Stack<string>();
            for (int i = 0; i < instruction.Count; i++)
            {
                var val = stacks[instruction.From].Pop();
                tempStack.Push(val);
            }

            while (tempStack.Any())
            {
                stacks[instruction.To].Push(tempStack.Pop());
            }
        });
        var resp = stacks.Where(x => x.Any()).Select(x => x.Pop()).Aggregate((i, j) => i + j);
        return resp;
    }

    private List<Instruction> GetInstructions()
    {
        Regex rx = new Regex(@"\d+");
        return _list.Where(x => x.StartsWith("move"))
            .Select(x =>
            {
                var match = rx.Matches(x);
                int count = int.Parse(match.ElementAt(0).Value);
                int from = int.Parse(match.ElementAt(1).Value);
                int to = int.Parse(match.ElementAt(2).Value);

                return new Instruction
                {
                    Count = count,
                    From = from,
                    To = to
                };
            })
            .ToList();
    }

    private Stack<string>[] CratesToStacks()
    {
        var crates = GetCrateList();

        Regex rx = new Regex("[a-zA-Z]");
        var crateCount = crates.Select(x => rx.Matches(x).Count).Max();
        var stacks = Enumerable.Range(0, crateCount + 1).Select(_ => new Stack<string>()).ToArray();
        crates.Reverse();
        crates.ForEach(x =>
        {
            foreach (Match match in rx.Matches(x))
            {
                var bucketId = (int) Math.Ceiling(((decimal) match.Index / 4));
                stacks[bucketId].Push(match.Value);
            }
        });

        return stacks;
    }

    private List<string> GetCrateList()
    {
        return _list.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).SkipLast(1).ToList();
    }
}

public class Instruction
{
    public int Count { get; set; }
    public int From { get; set; }
    public int To { get; set; }
}
