using AoC;
using AoC.Helpers;


var year = EnvironmentHelper.ParseYearInput();
var day = EnvironmentHelper.ParseDayInput();

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("====================================");
Console.WriteLine($"        Advent Of Code {year}");
Console.WriteLine("====================================");
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine($"============ Day {day} ================");
Console.ResetColor();
var taskType = Type.GetType($"AoC.Tasks.Year{year}.Day{day}.Solution");

if (taskType is null)
{
    Console.WriteLine($"Task from year: {year} & day: {day} doesn't exist.");
    return;
} 

var input = InputHelper.ReadTaskInput(year, day);

var task = (ISolver) Activator.CreateInstance(taskType, input)!;
var partOne = task.PartOne();
Console.WriteLine($"PartOne: {partOne}");
var partTwo = task.PartTwo();
Console.WriteLine($"PartTwo: {partTwo}");
