# ![Chessar.LongPaths](icon.png "Chessar.LongPaths")&nbsp;Chessar.LongPaths

[![Version](https://img.shields.io/nuget/v/Chessar.LongPaths.svg)](https://www.nuget.org/packages/Chessar.LongPaths)
[![Downloads](https://img.shields.io/nuget/dt/Chessar.LongPaths.svg)](https://www.nuget.org/packages/Chessar.LongPaths)
[![License](https://img.shields.io/:license-mit-blue.svg)](https://github.com/chessar/LongPaths/blob/master/LICENSE.md)
![Platforms](https://img.shields.io/badge/platform-windows-lightgray.svg)
![Language](https://img.shields.io/badge/language-c%23-orange.svg)
![Coverage](https://img.shields.io/badge/coverage-100%25-yellow.svg)

**Chessar.LongPaths** is a .NET library that allows you to enable long path support for the main
[`System.IO`](https://docs.microsoft.com/en-us/dotnet/api/system.io)
classes:

[`FileStream`](https://docs.microsoft.com/en-us/dotnet/api/system.io.filestream),
[`File`](https://docs.microsoft.com/en-us/dotnet/api/system.io.file),
[`FileInfo`](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo),
[`Directory`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory),
[`DirectoryInfo`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo), ...
(and others).

The library is based on replacing the internal
[`NormalizePath`](https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,390) and 
[`GetFullPathInternal`](https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,361)
functions from the static
[`Path`](https://docs.microsoft.com/en-us/dotnet/api/system.io.path)
class. The replacement is done using
[`JMP hooks`](https://github.com/wledfor2/PlayHooky)
(thanks to [**@wledfor2**](https://github.com/wledfor2)),
in which the long path prefix **`\\?\`** or **`\\?\UNC\`** is added.
Adding a prefix is done by calling the internal function
[`Path.AddLongPathPrefix`](https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,944).
Note also that the addition of such prefixes depends on the `UseLegacyPathHandling` and
`BlockLongPaths` settings, which must necessarily be `false` (in the
[`AppContextSwitchOverrides`](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/runtime/appcontextswitchoverrides-element) element).

In this case, your code does not need to directly add such prefixes to the paths.

# Supported Platforms:
* .NET Framework 4.6.2+ (see [.NET Blog](https://blogs.msdn.microsoft.com/dotnet/2016/08/02/announcing-net-framework-4-6-2/#bcl))

# How to use
1. Add the [Chessar.LongPaths](https://www.nuget.org/packages/Chessar.LongPaths/) NuGet package to the project.
2. In the file `app.config` (or `web.config`), in the section `runtime`, add:
```xml
<configuration>
...
  <runtime>
    <AppContextSwitchOverrides value="Switch.System.IO.UseLegacyPathHandling=false;Switch.System.IO.BlockLongPaths=false" />
  </runtime>
...
```
3. In the code (when you start the application or at the beginning of `Main`) add code:
```csharp
...
using static Chessar.Hooks;
...

    PatchLongPaths();

```
4. Usage
```csharp
...
var fileInfo = new FileInfo(path);
var fullName = fileInfo.FullName; // with long path prefix
...
```
5. At the end of the application:
```csharp
    RemoveLongPathsPatch();
```
See also [Examples](https://github.com/chessar/LongPaths/tree/master/Examples).

# Notes
Next methods does not work for long paths, even if a prefix is added:
* [`Directory.SetCurrentDirectory`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setcurrentdirectory)

# TODO
1. Add long path support in methods from [`Notes`](https://github.com/chessar/LongPaths#notes).
2. Make hooks more thread safe.

# License
MIT - See [LICENSE](https://github.com/chessar/LongPaths/blob/master/LICENSE.md)
