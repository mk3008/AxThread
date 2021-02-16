using AxThread;
using System.IO;

namespace FileExportSample
{
    internal class BFileExporter : IProcess
    {
        public DirectoryInfo ExportDirectory { get; set; }

        public string FileName { get; set; }

        public void Execute()
        {
            var path = Path.Combine(ExportDirectory.FullName, FileName);
            File.AppendAllText(path, "b");
        }
    }
}