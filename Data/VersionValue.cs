using AppsettingsVersion.Enums;

namespace AppsettingsVersion.Data;

public class VersionValue
{
    public int Major { get; private set; }
    public int Minor { get; private set; }
    public int Patch { get; private set; }
    public string Appendix { get; } = "";
    public VersionValue(string version)
    {
        var versionParts = version.Split('.');
        if (versionParts.Length < 3)
        {
            throw new Exception("invalid version value: " + version);
        }
        try
        {
            Major = int.Parse(versionParts[0]);
            Minor = int.Parse(versionParts[1]);
            Patch = int.Parse(versionParts[2]);
        }
        catch (Exception)
        {
            throw new Exception($"invalid verison value: {version}, version parts must be integers");
        }
        for (var k = 3; k < versionParts.Length; k++)
        {
            Appendix += $".{versionParts[k]}";
        }
    }
    public void Increment(IncrementMode mode)
    {
        if (mode == IncrementMode.Patch)
        {
            Patch += 1;
        }
        if (mode == IncrementMode.Minor)
        {
            Minor += 1;
        }
        if (mode == IncrementMode.Major)
        {
            Major += 1;
        }
    }
    public bool GreaterThan(VersionValue otherVersion)
    {
        if (Major > otherVersion.Major)
        {
            return true;
        }
        if (Major < otherVersion.Major)
        {
            return false;
        }
        if (Minor > otherVersion.Minor)
        {
            return true;
        }
        if (Minor < otherVersion.Minor)
        {
            return false;
        }
        if (Patch > otherVersion.Patch)
        {
            return true;
        }
        return false;
    }
    public bool EqualTo(VersionValue otherVersion)
    {
        return Major == otherVersion.Major && Minor == otherVersion.Minor && Patch == otherVersion.Patch;
    }
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}{Appendix}";
    }
}