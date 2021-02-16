using AxThread;
using System;
using System.Threading.Tasks;

namespace BreakfastSample
{
    internal class Juicer : IAsyncProcess
    {
        public async Task ExecuteAsync()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Pouring juice");
            });
        }
    }
}