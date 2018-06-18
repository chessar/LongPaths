// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Long Path Support")]
[assembly: AssemblyDescription(@"Chessar.LongPaths is a .NET library that allows you to enable long path support for the main System.IO classes (and others).
It is based on replacing with JMP hooks, internal functions in System.IO.Path (NormalizePath and GetFullPathInternal), which adds prefixes (\\?\ or \\?\UNC\) for paths to files/folders.
In this case, your code does not need to directly add such prefixes to the paths.

Supported platforms:
  .NET 4.6.2+
")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("")]
#endif
[assembly: AssemblyCompany("Chessar")]
[assembly: AssemblyProduct("Chessar.LongPaths")]
[assembly: AssemblyCopyright("Copyright © Chessar 2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0")]
[assembly: AssemblyFileVersion("1.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0-rc003")]
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
[assembly: CLSCompliant(true)]