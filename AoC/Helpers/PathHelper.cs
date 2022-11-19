namespace AoC.Helpers;

public static class PathHelper
{
    public static string GetProjectRootPath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var index = baseDirectory.IndexOf("bin", StringComparison.Ordinal);

        var projectRoot = baseDirectory;
        // If bin directory index is found
        if (index >= 0)
        {
            projectRoot = baseDirectory[..index];
        }

        return projectRoot;
    }
}
