using System.Threading.Tasks;

namespace AxThread
{
    internal class ProcessWrap : IAsyncProcess
    {
        /// <summary>
        /// sync process
        /// </summary>
        public IProcess Core { get; set; }

        /// <summary>
        /// execute process async
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Task.Run(() => Core.Execute());
        }
    }
}