using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNamePlate.Web.Common
{
    public class MemCache
    {
        private static MemCache _instance;

        private static Dictionary<string, object> _cache;

        public static MemCache Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new MemCache();
                }
                return _instance;
            }
        }

        private MemCache()
        {
            _cache = new Dictionary<string, object>();
        }

        public void AddOrReplace(string key, object item)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = item;
            }
            else 
            {
                _cache.Add(key, item);
            }
        }

        public object Get(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            else
            {
                return null;
            }
        }

    }
}
