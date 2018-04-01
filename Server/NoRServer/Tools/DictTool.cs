using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoRServer.Tools
{
    public class DictTool
    {
        public static void TryGetHandle<T1,T2>(Dictionary<T1,T2> dict,T1 key,out T2 value)
        {
            if(!dict.TryGetValue(key, out value))
            {
                value = default(T2);
            }
        }
    }
}
