using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxThread
{
    internal class ActionWrap : IAsyncProcess
    {
        /// <summary>
        /// sync process
        /// </summary>
        public Action Core { get; set; }

        /// <summary>
        /// execute process async
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Task.Run(() => Core.Invoke());
        }
    }
}