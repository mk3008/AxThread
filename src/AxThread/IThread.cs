using System;
using System.Collections.Generic;

namespace AxThread
{
    /// <summary>
    /// thread
    /// </summary>
    public interface IThread : IInjectableProcess
    {
        /// <summary>
        /// success end process count
        /// </summary>
        public int SuccessCount { get; }
    }
}