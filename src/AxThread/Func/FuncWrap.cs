using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxThread
{
    public class FuncWrap : IAsyncProcess
    {
        /// <summary>
        /// sync process
        /// </summary>
        public Func<Task> Core { get; set; }

        /// <summary>
        /// execute process async
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Core.Invoke();
        }
    }
}