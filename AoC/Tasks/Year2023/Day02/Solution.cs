using System.Text.RegularExpressions;

namespace AoC.Tasks.Year2023.Day02;

public class Solution(string input) : ISolver
{
    private readonly List<string> _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        const int red = 12;
        const int green = 13;
        const int blue = 14;

        var result = _list.Select(GetGameData())
        .Where(game => game is { Red: <= red, Green: <= green, Blue: <= blue })
        .Sum(x => x.Index);

        return result;
    }


    public object PartTwo()
    {
        var result = _list.Select(GetGameData())
            .Select(game => game.Red * game.Green * game.Blue)
            .Sum();
        
        return result;
    }
    
    private static Func<string, Game> GetGameData() =>
        x =>
        {
            var gameNumber = Regex.Matches(x, @"Game (\d+)").Select(index => int.Parse(index.Groups[1].Value)).First();
            var red = Regex.Matches(x, @"(\d+) red").Select(red => int.Parse(red.Groups[1].Value)).Max();
            var green = Regex.Matches(x, @"(\d+) green").Select(green => int.Parse(green.Groups[1].Value)).Max();
            var blue = Regex.Matches(x, @"(\d+) blue").Select(blue => int.Parse(blue.Groups[1].Value)).Max();
            return new Game(gameNumber, red, green, blue);
        };
}

public record Game(int Index, int Red, int Green, int Blue);

