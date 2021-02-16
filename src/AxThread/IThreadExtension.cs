using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace AxThread
{
    public static class IThreadExtension
    {
        public static int AllProcessCount(this IThread source)
        {
            var cnt = source.Processes.Select(x => (x is IThread) ? (x as IThread).AllProcessCount() : 1).Sum();
            return cnt;
        }

        public static int AllSuccessCount(this IThread source)
        {
            var cnt = source.Processes.Select(x => (x as IThread)?.SuccessCount).Sum();
            cnt += source.SuccessCount;
            return cnt.HasValue ? cnt.Value : 0;
        }

        public static int Progress(this IThread source)
        {
            var rate = source.AllSuccessCount() * 100 / source.AllProcessCount();
            return rate;
        }
    }
}