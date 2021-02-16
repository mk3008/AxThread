using AxThread;
using System;
using System.Threading.Tasks;

namespace BreakfastSample
{
    internal class BaconFryer : IAsyncProcess
    {
        public int Slices { get; set; }

        public async Task ExecuteAsync()
        {
            Console.WriteLine($"\tputting {Slices} slices of bacon in the pan");
            Console.WriteLine("\tcooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < Slices; slice++)
            {
                Console.WriteLine("\tflipping a slice of bacon");
            }
            Console.WriteLine("\tcooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("\tPut bacon on plate");
        }
    }
}