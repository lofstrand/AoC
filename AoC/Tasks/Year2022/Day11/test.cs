using System.Diagnostics;

namespace AoC.Tasks.Year2022.Day11;

public class Stuff
{
    public void Test()
    {
        var sw = new Stopwatch();
        sw.Start();
        var reader = new StreamReader(@"input.txt");
        const int ROUNDS = 10000;
        List<Monkey> monkeys = new List<Monkey>();

        Monkey currentMonkey = new Monkey();
        while (!reader?.EndOfStream ?? false)
        {
            var line = reader?.ReadLine()?.Trim().Split(':', ' ', ',');
            if (line.Length == 1) continue;
            switch ((line?[0], line?[1]))
            {
                case ("Monkey", _):
                    currentMonkey = new Monkey();
                    break;
                case ("Starting", _):
                    currentMonkey.items = new Queue<long>(line[2..].Select(x => long.TryParse(x, out long xInt) ? xInt : -1).Where(x => x > -1));
                    break;
                case ("Operation", _):
                    currentMonkey.operation = line[line.Length - 2] switch
                    {
                        "+" => x => x + (long.TryParse(line[line.Length - 1], out long a) ? a : x),
                        "-" => x => x - (long.TryParse(line[line.Length - 1], out long a) ? a : x),
                        "*" => x => x * (long.TryParse(line[line.Length - 1], out long a) ? a : x),
                        "/" => x => x / (long.TryParse(line[line.Length - 1], out long a) ? a : x),
                        _ => x => x
                    };
                    break;
                case ("Test", _):
                    currentMonkey.test = x => x % (long.TryParse(line[line.Length - 1], out long a) ? a : x) == 0;
                    break;
                case ("If", "true"):
                    currentMonkey.TrueMonkey = int.TryParse(line[line.Length - 1], out int t) ? t : -1;
                    break;
                case ("If", "false"):
                    currentMonkey.FalseMonkey = int.TryParse(line[line.Length - 1], out int f) ? f : -1;
                    monkeys.Add(currentMonkey);
                    break;
                default: break;
            }
        }

        foreach (var _ in Enumerable.Range(0, ROUNDS))
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.items.TryDequeue(out long worryLevel))
                {
                    worryLevel = monkey.operation!(worryLevel);
                    worryLevel %= 9699690; //Got a hint about LCM for this line :)
                    // worryLevel=(long)Math.Floor(worryLevel/3d);
                    if (monkey.test!(worryLevel))
                    {
                        monkeys[monkey.TrueMonkey].items.Enqueue(worryLevel);
                    }
                    else
                    {
                        monkeys[monkey.FalseMonkey].items.Enqueue(worryLevel);
                    }

                    monkey.numberOfInspections++;
                }
            }
        }

        var topTwoBusiestMonkeys = new List<Monkey>(monkeys.OrderByDescending(m => m.numberOfInspections).Take(2));
        var monkeyBusiness = topTwoBusiestMonkeys![0].numberOfInspections * topTwoBusiestMonkeys![1].numberOfInspections;
        reader?.Dispose();
        sw.Stop();
        Console.WriteLine($"\nPart 1: {monkeyBusiness}");
        Console.WriteLine($"Executed in {sw.Elapsed.TotalMilliseconds} ms.");
    }

    class Monkey
    {
        public Queue<long> items = new Queue<long>();
        public Func<long, long>? operation;
        public Predicate<long>? test;
        public int TrueMonkey;
        public int FalseMonkey;
        public long numberOfInspections = 0;
    }
}
