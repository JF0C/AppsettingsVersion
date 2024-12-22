using System.Text.RegularExpressions;
using AppsettingsVersion.Data;
using AppsettingsVersion.Enums;

namespace AppsettingsVersion.Services;

public partial class VersionIncrementer(string file = "./appsettings.json")
{
    [GeneratedRegex("^ *\"Version\" *: *\".*\",?$")]
    private static partial Regex VersionSection();
    public string CurrentVersion()
    {
        var lines = File.ReadAllLines(file);
        foreach (var line in lines)
        {
            var versionMatch = VersionSection().Match(line);
            if (versionMatch.Success)
            {
                return VersionValue(line);
            }
        }
        throw new Exception("Version not found in file " + file);
    }
    public string IncrementVersion(IncrementMode mode = IncrementMode.Minor, string? currentVersion = null)
    {
        var lines = File.ReadAllLines(file);
        var result = new List<string>();
        var version = "invalid";
        foreach (var line in lines)
        {
            var versionMatch = VersionSection().Match(line);
            if (versionMatch.Success)
            {
                result.Add(IncrementVersionSection(line, mode, currentVersion, out var newVersion));
                version = newVersion;
            }
            else
            {
                result.Add(line);
            }
        }
        
        File.WriteAllLines(file, result);
        return version;
    }
    private static string IncrementVersionSection(string section, IncrementMode mode, string? currentVersion, out string version)
    {
        var parts = section.Split(':');
        if (parts.Length != 2)
        {
            throw new Exception("invalid version section");
        }
        var versionString = VersionValue(section);
        var lineEnd = section.EndsWith(',') ? "," : "";
        var versionValue = new VersionValue(versionString);
        if (currentVersion == null)
        {
            versionValue.Increment(mode);
        }
        else
        {
            var currentVersionValue = new VersionValue(currentVersion);
            if (currentVersionValue.GreaterThan(versionValue))
            {
                versionValue = currentVersionValue;
                versionValue.Increment(mode);
            }
            else if (versionValue.EqualTo(currentVersionValue))
            {
                versionValue.Increment(mode);
            }
        }
        version = versionValue.ToString();
        return $"{parts[0]}: \"{version}\"{lineEnd}";
    }

    private static string VersionValue(string section)
    {
        var parts = section.Split(':');
        if (parts.Length != 2)
        {
            throw new Exception("invalid version section");
        }
        return  parts[1].Replace("\"", "").Replace(" ", "").Replace(",", "");
    }

}