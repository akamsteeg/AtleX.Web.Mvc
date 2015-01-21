using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtleX.Web.Mvc.OutputCache
{
    [Serializable]
    internal class OutputCacheItem
    {
        public string Key { get; set; }
        public byte[] Data { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
    }
}
