using System.Text.RegularExpressions;

namespace AoC.Helpers;

public static class EnvironmentHelper
{
    public static string ParseYearInput()
    {
        var year = Environment.GetEnvironmentVariable("YEAR");
        if (year == null)
        {
            return DateTime.UtcNow.Year.ToString();
        }

        var currentYear = DateTime.UtcNow.Year;
        var regex = new Regex($@"^201[5-9]|202[0-{currentYear.ToString().Last()}]$");
        if (!regex.Match(year).Success)
        {
            throw new ArgumentException($"Year {year} is not valid.");
        }

        return year;
    }

    public static string ParseDayInput()
    {
        var day = Environment.GetEnvironmentVariable("DAY");
        if (day == null)
        {
            return DateTime.UtcNow.Day.ToString("00");
        }

        var regex = new Regex(@"^0*([1-9]|1[0-9]|2[0-5])$");
        if (!regex.Match(day).Success)
        {
            throw new ArgumentException($"Day {day} is not valid.");
        }

        return day;
    }
}
