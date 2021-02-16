using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxThread
{
    /// <summary>
    /// Async porcess container
    /// </summary>
    public class AxProcessContainer : IAxProcess
    {
        /// <summary>
        /// logic name.
        /// it is primarily for logging.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// execute start event.
        /// it is primarily for logging.
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// execute success event.
        /// it is primarily for logging.
        /// </summary>
        public event EventHandler OnSuccess;

        /// <summary>
        /// execute fail event.
        /// it is primarily for logging.
        /// </summary>
        public event EventHandler OnFail;

        /// <summary>
        /// execute fail event.
        /// it is primarily for logging.
        /// </summary>
        public event EventHandler OnEnd;

        private bool IsSuccess { get; set; } = false;

        /// <summary>
        /// process list.
        /// this process
        /// </summary>
        public IList<IAxProcess> Processes { get; } = new List<IAxProcess>();

        /// <summary>
        /// shortcut method
        /// </summary>
        /// <param name="proc"></param>
        public void Regist(IAxProcess proc)
        {
            Processes.Add(proc);
        }

        /// <summary>
        /// parallel execute option.
        /// this option is 'true' then process list executed by parallel.
        /// </summary>
        public bool IsParallel { get; set; } = false;

        public int Progress => SuccessCount * 100 / ProcessCount;

        public int ProcessCount => Processes.Select(x => x.ProcessCount).Sum();

        public int SuccessCount => Processes.Select(x => x.SuccessCount).Sum();

        public async Task ExecuteAsync()
        {
            if (IsParallel)
            {
                await ExecuteParalleAsync();
            }
            else
            {
                await ExecuteSerialAsync();
            }
        }

        /// <summary>
        /// execute process as async(serial)
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSerialAsync()
        {
            OnStart?.Invoke(this, EventArgs.Empty);

            try
            {
                foreach (var item in Processes)
                {
                    await item.ExecuteAsync();
                }
                IsSuccess = true;
                OnSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                IsSuccess = false;
                OnFail?.Invoke(this, EventArgs.Empty);
                throw;
            }
            finally
            {
                OnEnd?.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task ExecuteParalleAsync()
        {
            OnStart?.Invoke(this, EventArgs.Empty);

            var lst = Processes.Select(x => x.ExecuteAsync());
            using (var tk = Task.WhenAll(lst))
            {
                try
                {
                    await tk;

                    IsSuccess = true;
                    OnSuccess?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception)
                {
                    IsSuccess = false;
                    foreach (var item in tk.Exception.InnerExceptions)
                    {
                        OnFail?.Invoke(this, EventArgs.Empty);
                    }
                    throw;
                }
                finally
                {
                    OnEnd?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}