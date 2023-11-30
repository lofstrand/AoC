using AoC;

var year = EnvironmentHelper.ParseYearInput();
var day = EnvironmentHelper.ParseDayInput();

Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine("====================================");
Console.WriteLine($"        Advent Of Code {year}");
Console.WriteLine($"             Day {day} ");
Console.WriteLine("====================================");
Console.ResetColor();

await InputHelper.GetInput(year, day);
var taskType = Type.GetType($"AoC.Tasks.Year{year}.Day{day}.Solution");
if (taskType is null)
{
    Console.WriteLine($"Task from year: {year} & day: {day} doesn't exist.");
    return;
}

await InputHelper.GetInput(year, day);
var input = InputHelper.ReadTaskInput(year, day);
var task = (ISolver) Activator.CreateInstance(taskType, input)!;

var partOne = task.PartOne();
Console.WriteLine($"PartOne: {partOne}");
var partTwo = task.PartTwo();
Console.WriteLine($"PartTwo: {partTwo}");
