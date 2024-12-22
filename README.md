# AppsettingsVersion

Increment the version inside a dotnet `appsettings.json` file.

## Prerequisites
A top-level entry semantic versioned value with the key "Version" must be present in the appsettings file: 
```
{
    "Version": "1.0.0"
}
```
## Usage
`dotnet AppsettingsVersion.dll ./appsettings.json -m minor -c 1.0.2`:
- The first argument (here `./appsettings.json`) is the appsettings file name (incl path)
- `-m` optional: specifies the mode meaning which part of the version to increment. Possible values are major, minor and patch
- `-c` optional: a version for comparison. If the version is already greater than the version of the compared version, nothing is done. This can be used to prevent accidental multiple version increments and synchronize with the version of a deployed application.
- `-r` if this flag is set, the current version is read out and returned, no changes are made. Other arguments are ignored.
