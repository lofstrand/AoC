using AoC.Helpers;

namespace AoC.Tasks.Year2022.Day10;

public class Solution : ISolver
{
    private readonly List<Instruction> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input)
        .Select(x =>
        {
            var strSplit = x.Split(" ");
            var command = strSplit[0];
            var value = (strSplit.Length > 1) ? int.Parse(strSplit[1]) : 0;
            return new Instruction { Command = command, Value = value };
        })
        .ToList();

    public object PartOne()
    {
        var cycles = GetCycles();

        var index = 20;
        var sum = 0;
        while (index <= 220)
        {
            var element = cycles.ElementAt(index);
            var total = (element * (index));
            Console.WriteLine($"{index} * {element} = {total}");
            sum += total;
            index += 40;
        }

        return sum;
    }

    public object PartTwo()
    {
        var cycles = GetCycles();
        var drawnCycles = new List<List<string>>();
        var tempList = Enumerable.Range(0, 40).ToList().Select(x => ".").ToList();
        for (int i = 0; i < cycles.Count; i++)
        {
            if (cycles.Count % 40 == 0)
            {
                drawnCycles.Add(tempList);
                tempList = Enumerable.Range(0, 40).ToList().Select(x => ".").ToList();
            }

            // tempList.ElementAt(i)

        }
        
        drawnCycles.ForEach(cList =>
        {
            cList.ForEach(Console.Write);
            Console.WriteLine();
        });
        return int.MinValue;
    }

    private List<int> GetCycles()
    {
        int x = 1;
        var cycles = new List<int> { x };
        _list.ForEach(instruction =>
        {
            switch (instruction.Command)
            {
                case "noop":
                    cycles.Add(x);
                    break;
                case "addx":
                    cycles.Add(x);
                    cycles.Add(x);
                    break;
            }

            x += instruction.Value;
        });
        return cycles;
    }
}

public class Instruction
{
    public string Command { get; set; }
    public int Value { get; set; }
}
