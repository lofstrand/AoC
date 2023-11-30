namespace AoC.Tasks.Year2022.Day02;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne() => RockPaperScissors(_list);
    public object PartTwo() => RockPaperScissorsP2(_list);

    private static int RockPaperScissors(IReadOnlyCollection<string> input)
    {
        var tuples = input.ToTupleList();
        var result = tuples.ToList()
            .Select(x =>
            {
                var (left, right) = (x.Item1.ToHand(), x.Item2.ToHand());
                return CalculateWinner(left, right);
            })
            .Sum();

        return result;
    }

    private static int RockPaperScissorsP2(IReadOnlyCollection<string> input)
    {
        var tuples = input.ToTupleList();
        var result = tuples.ToList()
            .Select(x =>
            {
                var (left, right) = (x.Item1.ToHand(), x.Item2.ToResult());
                return CalculateResult(left, right);
            })
            .Sum();

        return result;
    }

    private static int CalculateResult(HandType left, ResultType resultType)
    {
        int shape = (int) left;
        int outcome = 0;

        if (resultType == ResultType.Draw)
        {
            outcome = 3;
        }

        // Win
        if (resultType == ResultType.Win)
        {
            outcome = 6;
            switch (left)
            {
                case HandType.Scissor:
                    shape = (int) HandType.Rock;
                    break;
                case HandType.Paper:
                    shape = (int) HandType.Scissor;
                    break;
                case HandType.Rock:
                    shape = (int) HandType.Paper;
                    break;
            }
        }

        // Lose
        if (resultType == ResultType.Lose)
        {
            switch (left)
            {
                case HandType.Scissor:
                    shape = (int) HandType.Paper;
                    break;
                case HandType.Paper:
                    shape = (int) HandType.Rock;
                    break;
                case HandType.Rock:
                    shape = (int) HandType.Scissor;
                    break;
            }
        }

        return shape + outcome;
    }

    private static int CalculateWinner(HandType left, HandType right)
    {
        int shape = (int) right;
        // Loss is default 0
        int outcome = 0;

        // Draw
        if (left.Equals(right))
        {
            outcome = 3;
        }

        // Win
        switch (right)
        {
            case HandType.Scissor when left == HandType.Paper:
            case HandType.Paper when left == HandType.Rock:
            case HandType.Rock when left == HandType.Scissor:
            {
                outcome = 6;
                break;
            }
        }

        return shape + outcome;
    }
}

public enum HandType
{
    Rock = 1,
    Paper = 2,
    Scissor = 3,
    Unknown
}

public enum ResultType
{
    Win,
    Lose,
    Draw,
    Unknown
}

public static class StringExtensions
{
    public static HandType ToHand(this string val)
    {
        switch (val)
        {
            case "A":
                return HandType.Rock;
            case "B":
                return HandType.Paper;
            case "C":
                return HandType.Scissor;

            case "X":
                return HandType.Rock;
            case "Y":
                return HandType.Paper;
            case "Z":
                return HandType.Scissor;
            default:
                return HandType.Unknown;
        }
    }

    public static ResultType ToResult(this string val)
    {
        switch (val)
        {
            case "X":
                return ResultType.Lose;
            case "Y":
                return ResultType.Draw;
            case "Z":
                return ResultType.Win;
            default:
                return ResultType.Unknown;
        }
    }
}
