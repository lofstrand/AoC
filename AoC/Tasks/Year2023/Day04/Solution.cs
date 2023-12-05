using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AoC.Tasks.Year2023.Day04;

public class Solution : ISolver
{
    private readonly List<string> _list;

    public Solution(string input) => _list = InputHelper.ToStringList(input);

    public object PartOne()
    {
        var result = _list.Select(ExtractTicketInfo())
            .Select(card =>
            {
                var intersected = card.Ticket.Intersect(card.WinningNumbers);
                var count = intersected.Count();
                var sum = (int) Math.Pow(2, count - 1);
                return sum;
            })
            .Sum();

        return result;
    }

    public object PartTwo()
    {
        var tickets = _list.Select(ExtractTicketInfo()).ToList();
        for (var i = 0; i < tickets.Count; i++)
        {
            var ticket = tickets[i];
            var intersected = ticket.Ticket.Intersect(ticket.WinningNumbers);
            var count = intersected.Count();
            for (int j = 1; j <= count && j < tickets.Count; j++)
            {
                var tmp = tickets[i + j];
                tmp.Count = (tmp.Count + ticket.Count);
            }
        }

        var result = tickets.Select(x => x.Count).Sum();

        return result;
    }

    private Func<string, Card> ExtractTicketInfo() => x =>
    {
        var matches = Regex.Matches(x, @"Card\s*(\d+).*?:\s*([\d\s*]+)\|\s*([\d\s*]+)");
        var gameNumber = matches[0].Groups[1].Value;
        var start = matches[0].Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        var end = matches[0].Groups[3].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        return new Card(gameNumber, start, end);
    };
}

public class Card(string CardNumber, IEnumerable<int> Ticket, IEnumerable<int> WinningNumbers, int Count = 1)
{
    public string CardNumber { get; init; } = CardNumber;
    public IEnumerable<int> Ticket { get; init; } = Ticket;
    public IEnumerable<int> WinningNumbers { get; init; } = WinningNumbers;
    public int Count { get; set; } = Count;

    public void Deconstruct(out string CardNumber, out IEnumerable<int> Ticket, out IEnumerable<int> WinningNumbers, out int Count)
    {
        CardNumber = this.CardNumber;
        Ticket = this.Ticket;
        WinningNumbers = this.WinningNumbers;
        Count = this.Count;
    }
}
