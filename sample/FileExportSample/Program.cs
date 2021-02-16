using AxThread;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileExportSample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var proc = BuildMainProcess();
            await proc.ExecuteAsync();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("press any key to end.");
            Console.ReadKey();
        }

        private static IAxProcess BuildMainProcess()
        {
            var root = new AxProcessContainer();

            var appDir = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory;
            var tmp = new DirectoryInfo(Path.Combine(appDir.FullName, "tmp"));
            var output = new DirectoryInfo(Path.Combine(appDir.FullName, "output"));

            if (tmp.Exists) tmp.Delete(true);
            tmp.Create();
            if (!output.Exists) output.Create();

            var archiver = new Archiver { SourceDirecotry = tmp, ExportDirectory = output, FileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}.zip" };

            root.Regist(BuildExportProcess(tmp, output));
            root.Regist(archiver.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine("archived")));

            return root;
        }

        private static IAxProcess BuildExportProcess(DirectoryInfo tmp, DirectoryInfo output)
        {
            var procs = new AxProcessContainer() { IsParallel = true };
            var aExporter = new AFileExporter { ExportDirectory = tmp, FileName = "a.txt" };
            var bExporter = new BFileExporter { ExportDirectory = tmp, FileName = "b.txt" };

            procs.Regist(aExporter.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine("file a exported")));
            procs.Regist(bExporter.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine("file b exported")));

            return procs;
        }
    }
}