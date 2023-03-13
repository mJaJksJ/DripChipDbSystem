using System;
using System.IO;
using System.Reflection;

namespace Startup
{
    public class Paths
    {
        public static string AppName => "DripChipDbSystem";
        public static string WorkingDirectory { get; }
        public static string DocsFile { get; }

        static Paths()
        {
            WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new NullReferenceException();
            DocsFile = Path.Combine(WorkingDirectory, $"{AppName}.xml");
        }
    }
}
