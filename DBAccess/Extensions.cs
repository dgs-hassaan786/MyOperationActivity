using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess
{
    public static class Extensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static T FromJson<T>(this string o)
        {
            return JsonConvert.DeserializeObject<T>(o);
        }
    }
}
