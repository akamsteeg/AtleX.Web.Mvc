using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Caching;
using StackExchange.Redis;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AtleX.Web.Mvc.OutputCache.Providers
{
    public class RedisOutputCacheProvider : OutputCacheProvider
    {
        private static ConnectionMultiplexer _redis;

        private static int _databaseNumber;
        private static string _keyPrefix;

        private static readonly object _lock = new object();

        public RedisOutputCacheProvider()
        {
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = this.GetType().FullName;
                }

                if (_redis == null)
                {
                    if (config["host"] == null || string.IsNullOrEmpty(config["host"]))
                        throw new ConfigurationErrorsException("No Redis host specified");
                    if (config["databaseNumber"] == null || string.IsNullOrEmpty(config["host"]))
                        throw new ConfigurationErrorsException("No database number specified");
                    if (!int.TryParse(config["databaseNumber"], out _databaseNumber))
                        throw new ConfigurationErrorsException("Database number must be an integer");

                    _keyPrefix = config["keyPrefix"] ?? "";

                    _redis = ConnectionMultiplexer.Connect(config["host"]);

                    base.Initialize(name, config);
                }
            }
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            this.Set(key, entry, utcExpiry);

            return entry;
        }

        public override object Get(string key)
        {
            IDatabase redisDb = _redis.GetDatabase(_databaseNumber);

            RedisValue storedItem = redisDb.StringGet(CreateStoreKey(key));

            object result = null;
            if (!storedItem.IsNull)
            {
                result = this.DeserializeItem(storedItem);
            }

            return result;
        }

        public override void Remove(string key)
        {
            IDatabase redisDb = _redis.GetDatabase(_databaseNumber);
            redisDb.KeyDelete(CreateStoreKey(key));
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            byte[] serializedItem = this.SerializeItem(entry);

            IDatabase redisDb = _redis.GetDatabase(_databaseNumber);
            redisDb.StringSet(CreateStoreKey(key), serializedItem, utcExpiry.Subtract(DateTime.UtcNow));
        }

        protected virtual byte[] SerializeItem(object item)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                bf.Serialize(stream, item);

                return stream.ToArray();
            }
        }

        protected virtual object DeserializeItem(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                object result = bf.Deserialize(stream);

                return result;
            }
        }

        private static string CreateStoreKey(string key)
        {
            return  _keyPrefix + key;
        }
    }
}
