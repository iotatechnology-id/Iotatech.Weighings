using System.Reflection;

[assembly: AssemblyVersion("1.0.1")]
[assembly: AssemblyProductAttribute("Iotatech.Weighings")]
[assembly: AssemblyTitleAttribute("Iotatech.Weighings")]
[assembly: AssemblyDescriptionAttribute(
    "Collection of weighing module abstractions for .NET commonly used in Iotatechnology projects.")]
[assembly: AssemblyCompanyAttribute("Seiko Santana")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif RELEASE
[assembly: AssemblyConfiguration("Release")]
#endif