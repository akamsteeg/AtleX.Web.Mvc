using AtleX.Web.Mvc.OutputCache.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtleX.Web.Mvc.Tests.OutputCache.Providers
{
    [TestFixture, Explicit("No Redis server available by default")]
    class RedisOutputCacheProviderTests
    {
        private RedisOutputCacheProvider _redisProvider;

        [SetUp]
        public void SetUp()
        {
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("connectionStringReference", ConfigurationManager.AppSettings["redisConnectionStringReference"]);
            nvc.Add("databaseNumber", ConfigurationManager.AppSettings["redisDatabaseNumber"]);
            nvc.Add("keyPrefix", ConfigurationManager.AppSettings["redisKeyPrefix"]);

            _redisProvider = new RedisOutputCacheProvider();
            _redisProvider.Initialize("", nvc);
        }

        [Test]
        public void StoreOneItemAndRetrieveIt()
        {
            // Arrange
            Guid data = Guid.NewGuid();

            // Act
            _redisProvider.Set(data.ToString("N"), data, DateTime.UtcNow.AddMinutes(5));
            object retrievedValue = _redisProvider.Get(data.ToString("N"));

            // Assert
            Assert.IsNotNull(retrievedValue);
            Assert.AreEqual(data, (Guid)retrievedValue);
        }

        [Test]
        public void RetrieveUnknownItem()
        {
            // Arrange
            Guid data = Guid.NewGuid();

            // Act
            object retrievedValue = _redisProvider.Get(data.ToString("N"));

            // Assert
            Assert.IsNull(retrievedValue);
        }

        [Test]
        public void StoreOneItemAndRemoveIt()
        {
            // Arrange
            Guid data = Guid.NewGuid();

            // Act
            _redisProvider.Set(data.ToString("N"), data, DateTime.UtcNow.AddMinutes(5));
            _redisProvider.Remove(data.ToString("N"));

            // Assert
            object retrievedValue = _redisProvider.Get(data.ToString("N"));

            Assert.IsNull(retrievedValue);
        }

        [Test,
        Sequential]
        public void StoreManyItemsAndRetrieveThem([Values(10, 100, 1000)] int amount)
        {
            // Arrange
            DateTime expiry = DateTime.UtcNow.AddMinutes(10);

            // Act
            for (int i = 0; i < amount; i++)
            {
                _redisProvider.Set(i.ToString(), i, expiry); 
            }

            // Assert
            for (int i = 0; i < amount; i++)
            {
                object retrievedValue = _redisProvider.Get(i.ToString());
                
                Assert.IsNotNull(retrievedValue);
                Assert.AreEqual(i, (int)retrievedValue);
            }
        }

        [Test]
        public void StoreItemWithExpiryRetrieveItWhenExpired()
        {
            // Arrange
            Guid data = Guid.NewGuid();

            int expireIn = 1; // seconds

            // Act
            _redisProvider.Set(data.ToString("N"), data, DateTime.UtcNow.AddSeconds(expireIn));
            Thread.Sleep(2 * 2 * 1000); // Check in 2 * the expiration time

            // Assert
            object retrievedValue = _redisProvider.Get(data.ToString("N"));

            Assert.IsNull(retrievedValue);
        }
    }
}
