using AppsettingsVersion.Enums;
using AppsettingsVersion.Services;

namespace AppsettingsVersion;

public static class Program
{
    public static void Main(string[] args)
    {
        var file = "appsettings.json";

        var argtype = 0;
        var mode = IncrementMode.Minor;
        var read = false;
        string? compareVersion = null;

        foreach (var arg in args)
        {
            if (arg == "-m")
            {
                argtype = 1;
                continue;
            }
            if (arg == "-r")
            {
                read = true;
                continue;
            }
            if (arg == "-c")
            {
                argtype = 2;
            }
            if (argtype == 0)
            {
                file = arg;
            }
            else if (argtype == 1)
            {
                mode = arg.ToLower() switch
                {
                    "major" => IncrementMode.Major,
                    "minor" => IncrementMode.Minor,
                    "patch" => IncrementMode.Patch,
                    _ => throw new Exception("invalid version increment type, must be [major, minor, patch]"),
                };
            }
            else if (argtype == 2)
            {
                compareVersion = arg;
            }
        }

        var incrementer = new VersionIncrementer(file);
        if (read)
        {
            Console.WriteLine(incrementer.CurrentVersion());
            return;
        }
        var version = incrementer.IncrementVersion(mode, compareVersion);
        Console.WriteLine(version);
    }
}

