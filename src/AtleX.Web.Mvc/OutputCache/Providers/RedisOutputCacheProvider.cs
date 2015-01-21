using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using StackExchange.Redis;

namespace AtleX.Web.Mvc.OutputCache.Providers
{
    public class RedisOutputCacheProvider : OutputCacheProvider
    {
        public RedisOutputCacheProvider()
        {
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            throw new NotImplementedException();
        }

        public override object Get(string key)
        {
            throw new NotImplementedException();
        }

        public override void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            throw new NotImplementedException();
        }
    }
}
