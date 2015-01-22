using StackExchange.Redis;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Caching;

namespace AtleX.Web.Mvc.OutputCache.Providers
{
    public class RedisOutputCacheProvider : OutputCacheProvider
    {
        /// <summary>
        /// The connection multiplexer is stored per the recommendations
        /// from Stack Exchange:
        /// 
        /// "Because the ConnectionMultiplexer does a lot, it is designed to be 
        /// shared and reused between callers"
        /// </summary>
        private static ConnectionMultiplexer _redis;
        private static int _databaseNumber;
        private static string _keyPrefix;

        private static readonly object _lock = new object();

        public RedisOutputCacheProvider()
        {
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            lock (_lock)
            {
                if (_redis == null)
                {
                    if (config["connectionStringReference"] == null || string.IsNullOrEmpty(config["connectionStringReference"]))
                        throw new ConfigurationErrorsException("No connectionStringReference specified");
                    if (ConfigurationManager.ConnectionStrings[config["connectionStringReference"]] == null)
                        throw new ConfigurationErrorsException(string.Format("No connection string with name '{0}' found", config["connectionStringReference"]));

                    if (config["databaseNumber"] == null || !int.TryParse(config["databaseNumber"], out _databaseNumber))
                    {
                        _databaseNumber = 0;
                    }

                    if (string.IsNullOrEmpty(name))
                    {
                        name = this.GetType().FullName;
                    }

                    _keyPrefix = config["keyPrefix"] ?? "";

                    string connectionString = ConfigurationManager.ConnectionStrings[config["connectionStringReference"]].ConnectionString;
                    _redis = ConnectionMultiplexer.Connect(connectionString);

                    base.Initialize(name, config);
                }
            }
        }

        /// <summary>
        /// Add a new object to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <param name="utcExpiry"></param>
        /// <returns>The added object</returns>
        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            this.Set(key, entry, utcExpiry);

            return entry;
        }

        /// <summary>
        /// Retrieve an object from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The requested object, or null if it does not exist in the cache</returns>
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

        /// <summary>
        /// Remove an object from the cache
        /// </summary>
        /// <param name="key"></param>
        public override void Remove(string key)
        {
            IDatabase redisDb = _redis.GetDatabase(_databaseNumber);
            redisDb.KeyDelete(CreateStoreKey(key));
        }

        /// <summary>
        /// Add a new object to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <param name="utcExpiry"></param>
        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            byte[] serializedItem = this.SerializeItem(entry);

            IDatabase redisDb = _redis.GetDatabase(_databaseNumber);
            redisDb.StringSet(CreateStoreKey(key), serializedItem, utcExpiry.Subtract(DateTime.UtcNow));
        }

        /// <summary>
        /// Serialize an object to store so it can be
        /// saved into the Redis cache
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual byte[] SerializeItem(object item)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                bf.Serialize(stream, item);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserialize an object retrieved from the cache
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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
            return _keyPrefix + key;
        }
    }
}
