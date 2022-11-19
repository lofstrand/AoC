using System.Diagnostics;

namespace AoC.Helpers;

public static class MethodHelper
{
    public static object? LaunchMethod(Type type, string input, string methodName)
    {
        var task = Activator.CreateInstance(type);
        var methodInfo = type.GetMethod(methodName);
        if (methodInfo == null)
        {
            return null;
        }

        var stopwatch = Stopwatch.StartNew();
        var result = methodInfo.Invoke(task, new object[] { input });
        stopwatch.Stop();

        // Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} milliseconds");
        return result;
    }
}
