# Chessar.LongPaths

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
[`NormalizePath`](https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,390)
and
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
For the following list of ctors/methods, you must directly specify the prefix of long paths
(because they are not supported by this library, see [Unit Tests](https://github.com/chessar/LongPaths/tree/master/UnitTests)):
* [`new DirectorySecurity(String, AccessControlSections)`](https://docs.microsoft.com/en-us/dotnet/api/system.security.accesscontrol.directorysecurity.-ctor#System_Security_AccessControl_DirectorySecurity__ctor_System_String_System_Security_AccessControl_AccessControlSections_)
* [`new FileSecurity(String, AccessControlSections)`](https://docs.microsoft.com/en-us/dotnet/api/system.security.accesscontrol.filesecurity.-ctor#System_Security_AccessControl_FileSecurity__ctor_System_String_System_Security_AccessControl_AccessControlSections_)
* [`Directory.GetAccessControl(String[, AccessControlSections])`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getaccesscontrol)
* [`File.GetAccessControl(String[, AccessControlSections])`](https://docs.microsoft.com/en-us/dotnet/api/system.io.file.getaccesscontrol)
* [`Directory.Move(String, String)`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.move)
* [`DirectoryInfo.MoveTo(String)`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo.moveto)

for example:
```csharp
...
using static Chessar.Hooks;
...
    var ds = new DirectorySecurity(path.AddLongPathPrefix(), acs);
```
or use [`DirectoryInfo`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directoryinfo) instead `Directory` (exclude `MoveTo` method) and
[`FileInfo`](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo) instead `File`.

**Note** that, next methods does not work for long paths, even if a prefix is added:
* [`Directory.SetCurrentDirectory`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.setcurrentdirectory)
* [`Image.Save(String)`](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.image.save) (use `Image.Save` to `Stream` instead)

# TODO
1. Speed up MethodInfo.Invoke in the class [`Hooks`](https://github.com/chessar/LongPaths/blob/master/src/Hooks.cs), using, for example, [`DynamicMethod.CreateDelegate`](https://docs.microsoft.com/ru-ru/dotnet/api/system.reflection.emit.dynamicmethod.createdelegate#System_Reflection_Emit_DynamicMethod_CreateDelegate_System_Type_System_Object_).
2. Add long path support in ctors/methods from [`Notes`](https://github.com/chessar/LongPaths#notes).
3. Add more unit tests.
4. Make hooks more thread safe.

# License
MIT - See [LICENSE](https://github.com/chessar/LongPaths/blob/master/LICENSE.md)
