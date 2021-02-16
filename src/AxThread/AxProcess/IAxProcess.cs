using System;

namespace AxThread
{
    public interface IAxProcess : IAsyncProcess
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

        public int ProcessCount { get; }

        public int SuccessCount { get; }

        public int Progress { get; }

        //public IEnumerable<IAxProcess> GetProcesses();
    }
}