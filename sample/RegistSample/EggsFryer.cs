using AxThread;
using System;
using System.Threading.Tasks;

namespace BreakfastSample
{
    internal class EggsFryer : IAsyncProcess
    {
        public int HowMany { get; set; }

        public async Task ExecuteAsync()
        {
            Console.WriteLine("\t\tWarming the egg pan...");
            await Task.Delay(1000);
            Console.WriteLine($"\t\tcracking {HowMany} eggs");
            Console.WriteLine("\t\tcooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("\t\tPut eggs on plate");
        }
    }
}