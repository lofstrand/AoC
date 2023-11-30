namespace AoC.Tasks.Year2022.Day09;

public class Solution : ISolver
{
    private readonly List<Move> _input;

    public Solution(string input)
    {
        _input = InputHelper
            .ToStringList(input)
            .ToTupleList(" ")
            .Select(x => new Move() { Direction = x.Item1, Distance = int.Parse(x.Item2) })
            .ToList();
    }

    public object PartOne()
    {
        return VisitedRopeKnots(2);
    }

    public object PartTwo()
    {
        return VisitedRopeKnots(10);
    }

    public int VisitedRopeKnots(int knots)
    {
        var rope = new (int X, int Y)[knots];
        var visited = new HashSet<(int, int)>();

        foreach (var move in _input)
        {
            for (int i = 0; i < move.Distance; i++)
            {
                rope[0] = move.Direction switch
                {
                    "U" => (rope[0].X, --rope[0].Y),
                    "D" => (rope[0].X, ++rope[0].Y),
                    "R" => (++rope[0].X, rope[0].Y),
                    "L" => (--rope[0].X, rope[0].Y),
                    _ => throw new ArgumentOutOfRangeException()
                };

                for (int j = 1; j < rope.Length; j++)
                {
                    var xDist = rope[j - 1].X - rope[j].X;
                    var yDist = rope[j - 1].Y - rope[j].Y;

                    if (Math.Abs(xDist) > 1 || Math.Abs(yDist) > 1)
                    {
                        rope[j].X += Math.Sign(xDist);
                        rope[j].Y += Math.Sign(yDist);
                    }
                }

                visited.Add(rope.Last());
            }
        }

        return visited.Count;
    }
}

public class Move
{
    public int Distance { get; set; }
    public string Direction { get; set; } = string.Empty;
}
