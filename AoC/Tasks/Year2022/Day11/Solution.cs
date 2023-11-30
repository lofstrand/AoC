using System.Text.RegularExpressions;
using AoC.Helpers;

namespace AoC.Tasks.Year2022.Day11;

public class Solution : ISolver
{
    private readonly string _input;

    public Solution(string input) => _input = input;

    public object PartOne()
    {
        var monkeys = GenerateMonkeys();
        DoRoundsPart1(monkeys, 20, 3);

        var sum = monkeys.OrderByDescending(x => x.InspectCount).Select(x => x.InspectCount).Take(2).Aggregate((x, y) => x * y);
        return sum;
    }

    public object PartTwo()
    {
        var sum = 0:
        for (int i = 0; i < 50; i++)
        {
            var monkeys = GenerateMonkeys();
            DoRoundsPart1(monkeys, 10_000, i);
            sum = monkeys.Select(x => x.InspectCount).OrderByDescending(x => x).Sum();
        }

        return sum;
        // 849541615 is not right
        // 1156658008 too low
        // 1816574648 too low
    }

    private Monkey[] GenerateMonkeys()
    {
        var monkeys = InputHelper.ToStringList(_input)
            .ChunkList(7)
            .Select(chunk =>
            {
                var items = Regex.Matches(chunk[1], @"\d+").Select(match => long.Parse(match.Value)).ToList();
                var operation = chunk[2].Substring(19).Split(" ");

                return new Monkey
                {
                    Items = new Queue<long>(items),
                    Operation = operation[1] switch
                    {
                        "+" => x => x + (long.TryParse(operation[2], out long a) ? a : x),
                        "-" => x => x - (long.TryParse(operation[2], out long a) ? a : x),
                        "*" => x => x * (long.TryParse(operation[2], out long a) ? a : x),
                        "/" => x => x / (long.TryParse(operation[2], out long a) ? a : x),
                        _ => x => x
                    },
                    Test = x => x % (long.TryParse(Regex.Match(chunk[3], @"\d+").Value, out long a) ? a : x) == 0,
                    TrueMonkey = int.Parse(Regex.Match(chunk[4], @"\d+").Value),
                    FalseMonkey = int.Parse(Regex.Match(chunk[5], @"\d+").Value),
                    DivisibleBy = long.Parse(Regex.Match(chunk[3], @"\d+").Value)
                };
            })
            .ToArray();

        return monkeys;
    }

    private static void DoRoundsPart1(IReadOnlyList<Monkey> monkeys, int roundCount, long divisibleBy = 1)
    {
        for (int i = 0; i < roundCount; i++)
        {
            monkeys.ToList()
                .ForEach(monkey =>
                {
                    while (monkey.Items.TryDequeue(out long worryLevel))
                    {
                        worryLevel = monkey.Operation(worryLevel);
                        worryLevel /= divisibleBy;

                        if (monkey.Test(worryLevel))
                        {
                            monkeys[monkey.TrueMonkey].Items.Enqueue(worryLevel);
                        }
                        else
                        {
                            monkeys[monkey.FalseMonkey].Items.Enqueue(worryLevel);
                        }

                        monkey.InspectCount++;
                    }
                });
        }
    }

    private static void DoRoundsPart2(IReadOnlyList<Monkey> monkeys, int roundCount)
    {
        long superModulo = 1;
        monkeys.ToList().Select(x => x.DivisibleBy).ToList().ForEach(x => superModulo += x);
        superModulo %= monkeys.Count;
        for (int i = 0; i < roundCount; i++)
        {
            monkeys.ToList()
                .ForEach(monkey =>
                {
                    while (monkey.Items.TryDequeue(out long worryLevel))
                    {
                        worryLevel = monkey.Operation(worryLevel);
                        worryLevel /= superModulo;

                        if (monkey.Test(worryLevel))
                        {
                            monkeys[monkey.TrueMonkey].Items.Enqueue(worryLevel);
                        }
                        else
                        {
                            monkeys[monkey.FalseMonkey].Items.Enqueue(worryLevel);
                        }

                        monkey.InspectCount++;
                    }
                });
        }
    }
}

public class Monkey
{
    public Queue<long> Items { get; init; } = new();
    public string ItemList => string.Join(", ", Items.ToList().Select(s => s.ToString()));
    public required int TrueMonkey { get; init; }
    public required int FalseMonkey { get; init; }
    public required Func<long, long> Operation;
    public required Predicate<long> Test = null!;
    public required long DivisibleBy { get; init; }
    public long InspectCount { get; set; } = 0;
}
