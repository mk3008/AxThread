using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxThread
{
    /// <summary>
    /// parallel thread
    /// </summary>
    public class ParallelThread : IThread
    {
        public ParallelThread()
        {
            OnSuccess += (obj, e) => SuccessCount += 1;
        }

        /// <summary>
        /// logic name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// execute start event
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// execute success event
        /// </summary>
        public event EventHandler OnSuccess;

        /// <summary>
        /// execute fail event
        /// </summary>
        public event EventHandler OnFail;

        /// <summary>
        /// execute fail event
        /// </summary>
        public event EventHandler OnEnd;

        /// <summary>
        /// process list
        /// </summary>
        internal IList<IThread> InnerThread { get; } = new List<IThread>();

        /// <summary>
        /// process list
        /// </summary>
        IList<IThread> IThread.InnerThread => InnerThread;

        /// <summary>
        /// success end process count
        /// </summary>
        public int SuccessCount { get; private set; } = 0;

        /// <summary>
        /// execute process as async(parallel)
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            OnStart?.Invoke(this, EventArgs.Empty);

            var lst = InnerThread.Select(x => x.ExecuteAsync());
            using (var tk = Task.WhenAll(lst))
            {
                try
                {
                    await tk;
                }
                catch (Exception)
                {
                    foreach (var item in tk.Exception.InnerExceptions)
                    {
                        OnFail?.Invoke(this, EventArgs.Empty);
                    }
                    OnEnd?.Invoke(this, EventArgs.Empty);
                    throw;
                }
            }
            OnSuccess?.Invoke(this, EventArgs.Empty);
            OnEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}