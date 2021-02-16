using AxThread;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// https://docs.microsoft.com/ja-jp/dotnet/csharp/programming-guide/concepts/async/
/// </summary>
namespace BreakfastSample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var proc = BuildProcess();
            await proc.ExecuteAsync();

            Console.WriteLine("Breakfast is ready!");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("press any key to end.");
            Console.ReadKey();
        }

        private static IAxProcess BuildProcess()
        {
            var root = new AxProcessContainer();
            var burnProcs = new AxProcessContainer();

            burnProcs.IsParallel = true; //6sec
            //burnProcs.IsParallel = false; //14sec

            var coffee = new CoffeeMaker();
            var egg = new EggsFryer() { HowMany = 2 };
            var bacon = new BaconFryer() { Slices = 1 };
            var toast = new Toaster() { Slices = 2 };
            var juice = new Juicer();

            burnProcs.Regist(bacon.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine($"\tbacon is ready({root.Progress}%)")));
            burnProcs.Regist(egg.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine($"\t\teggs are ready({root.Progress}%)")));
            burnProcs.Regist(toast.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine($"\t\t\ttoast is ready({root.Progress}%)")));

            root.Regist(coffee.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine($"coffee is ready({root.Progress}%)")));
            root.Regist(burnProcs);
            root.Regist(juice.ToAxProcess(onSuccess: (obj, e) => Console.WriteLine($"juice is ready({root.Progress}%)")));

            var sw = new Stopwatch();
            root.OnStart += new EventHandler((obj, e) => sw.Start());
            root.OnEnd += new EventHandler((obj, e) => Console.WriteLine($"elapsed:{sw.ElapsedMilliseconds:#,###}msec"));

            return root;
        }
    }
}