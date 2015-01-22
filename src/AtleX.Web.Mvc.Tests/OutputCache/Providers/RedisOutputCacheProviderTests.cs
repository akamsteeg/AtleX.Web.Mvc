using AtleX.Web.Mvc.OutputCache.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtleX.Web.Mvc.Tests.OutputCache.Providers
{
    [TestFixture]
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
    }
}
