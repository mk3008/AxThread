using System.Threading.Tasks;

namespace AxThread
{
    /// <summary>
    /// ASync process interface
    /// </summary>
    public interface IAsyncProcess
    {
        /// <summary>
        /// execute process async
        /// </summary>
        /// <returns></returns>
        public Task ExecuteAsync();
    }
}