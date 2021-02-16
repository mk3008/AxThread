using System;

namespace AxThread
{
    public static class AsyncProcessExtension
    {
        /// <summary>
        /// convert to serial thread
        /// </summary>
        /// <param name="source"></param>
        /// <param name="onStart"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        /// <param name="onEnd"></param>
        /// <returns></returns>
        public static AxProcess ToAxProcess(this IAsyncProcess source, Action<object, EventArgs> onStart = null, Action<object, EventArgs> onSuccess = null, Action<object, EventArgs> onFail = null, Action<object, EventArgs> onEnd = null)
        {
            var st = new AxProcess();
            if (onStart != null) st.OnStart += new EventHandler((obj, e) => onStart(obj, e));
            if (onSuccess != null) st.OnSuccess += new EventHandler((obj, e) => onSuccess(obj, e));
            if (onFail != null) st.OnFail += new EventHandler((obj, e) => onFail(obj, e));
            if (onEnd != null) st.OnEnd += new EventHandler((obj, e) => onEnd(obj, e));

            st.Process = source;
            return st;
        }
    }
}