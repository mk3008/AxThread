using System;
using System.Threading.Tasks;

namespace AxThread
{
    public class AxProcess : IAxProcess
    {
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
        /// execute end event
        /// </summary>
        public event EventHandler OnEnd;

        private bool IsSuccess { get; set; } = false;

        public int ProcessCount => 1;

        public int SuccessCount => IsSuccess ? ProcessCount : 0;

        public int Progress => SuccessCount * 100 / ProcessCount;

        public IAsyncProcess Process { get; set; }

        public async Task ExecuteAsync()
        {
            OnStart?.Invoke(this, EventArgs.Empty);

            try
            {
                await Process.ExecuteAsync();
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
    }
}