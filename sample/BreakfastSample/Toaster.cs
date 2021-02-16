using AxThread;
using System;
using System.Threading.Tasks;

namespace BreakfastSample
{
    internal class Toaster : IAsyncProcess
    {
        public int Slices { get; set; }

        public async Task ExecuteAsync()
        {
            for (int slice = 0; slice < Slices; slice++)
            {
                Console.WriteLine("\t\t\tPutting a slice of bread in the toaster");
            }
            Console.WriteLine("\t\t\tStart toasting...");
            await Task.Delay(4000);
            Console.WriteLine("\t\t\tRemove toast from toaster");
        }
    }
}