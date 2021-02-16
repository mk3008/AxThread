using AxThread;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace FileExportSample
{
    internal class Archiver : IProcess
    {
        public DirectoryInfo SourceDirecotry { get; set; }

        public DirectoryInfo ExportDirectory { get; set; }

        public string FileName { get; set; }

        public void Execute()
        {
            var path = Path.Combine(ExportDirectory.FullName, FileName);
            ZipFile.CreateFromDirectory(SourceDirecotry.FullName, path);
        }
    }
}