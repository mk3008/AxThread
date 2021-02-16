using System.Threading.Tasks;

namespace AxThread
{
    /// <summary>
    /// warpping Process used to AsyncProcess
    /// </summary>
    internal class AxProcess : IAsyncProcess
    {
        /// <summary>
        /// sync process
        /// </summary>
        public IProcess Process { get; set; }

        /// <summary>
        /// execute process async
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Task.Run(() => Process.Execute());
        }
    }
}