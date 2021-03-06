![PackCheck-Logo](https://github.com/eisnstein/PackCheck/blob/main/PackCheck/Assets/icon.png)

# PackCheck

[![NuGet Package](https://img.shields.io/nuget/vpre/PackCheck)](https://nuget.org/packages/PackCheck)
[![License](https://img.shields.io/github/license/eisnstein/PackCheck)](https://github.com/eisnstein/PackCheck/blob/main/LICENSE)

Check for newer versions of installed NuGet Packages in your Terminal.

---

PackCheck is a dotnet tool for checking versions of installed NuGet packages in your .NET projects in your terminal.
The `check` command (default) shows you all NuGet packages in a nice table with the *current*, *latest stable* and *latest* versions of each package.
You can upgrade the .csproj file with the `upgrade` command to your desired versions. Whether to the *latest stable* or *latest* version, only a specific
package or all at once.      


## Installation

You can install PackCheck as a dotnet tool via NuGet:
 ```
 dotnet tool install --global PackCheck
 ```

## Usage

In your terminal cd to a .NET project and:

```
packcheck
```

This should give you something like this:

![PackCheck check example](https://github.com/eisnstein/PackCheck/blob/main/PackCheck/Assets/packcheck-check.png)

After that you can upgrade the package versions in the .csproj file by running:
> This changes your .csproj file!

```
packcheck upgrade
```

For a dry-run, which outputs the .csproj file into the terminal without actually changing the .csproj file, run:

```
packcheck upgrade --dry-run
```

For help run:

```
packcheck -h
```

## Color Highlighting

| Color | Description |
| ----- | ------------ |
| red | Major (Breaking changes) |
| yellow | Minor (New features, but backwards compatible) |
| green | Patch (Backwards compatible bug fixes only) |

## LICENSE

MIT