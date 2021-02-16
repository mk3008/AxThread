using AxThread;
using System;

namespace BreakfastSample
{
    internal class CoffeeMaker : IProcess
    {
        public void Execute()
        {
            Console.WriteLine("Pouring coffee");
        }
    }
}