using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AxThread
{
    public class Process : IAxProcess
    {
        public string Name { get; set; }

        public bool IsSuccessed { get; set; }

        public event EventHandler OnStart;

        public event EventHandler OnSuccess;

        public event EventHandler OnFail;

        public event EventHandler OnEnd;

        public IAsyncProcess ProcessCore { get; set; }

        public int ProcessCount => 1;

        public int SuccessCount => IsSuccessed ? 1 : 0;

        public int Progress => SuccessCount * 100 / ProcessCount;

        public async Task ExecuteAsync()
        {
            OnStart?.Invoke(this, EventArgs.Empty);

            try
            {
                await ProcessCore.ExecuteAsync();
                IsSuccessed = true;

                OnSuccess?.Invoke(this, EventArgs.Empty);
                OnEnd?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                OnFail?.Invoke(this, EventArgs.Empty);
                OnEnd?.Invoke(this, EventArgs.Empty);
                throw;
            }
        }

        public IEnumerable<IAxProcess> GetProcesses()
        {
            yield return this;
        }
    }
}