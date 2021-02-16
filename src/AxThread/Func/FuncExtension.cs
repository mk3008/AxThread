using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxThread
{
    public static class FuncExtension
    {
        public static AxProcess ToAxProcess(this Func<Task> source, Action<object, EventArgs> onStart = null, Action<object, EventArgs> onSuccess = null, Action<object, EventArgs> onFail = null, Action<object, EventArgs> onEnd = null)
        {
            var st = new AxProcess();
            if (onStart != null) st.OnStart += new EventHandler((obj, e) => onStart(obj, e));
            if (onSuccess != null) st.OnSuccess += new EventHandler((obj, e) => onSuccess(obj, e));
            if (onFail != null) st.OnFail += new EventHandler((obj, e) => onFail(obj, e));
            if (onEnd != null) st.OnEnd += new EventHandler((obj, e) => onEnd(obj, e));

            var ap = new FuncWrap() { Core = source };
            st.Process = ap;
            return st;
        }
    }
}