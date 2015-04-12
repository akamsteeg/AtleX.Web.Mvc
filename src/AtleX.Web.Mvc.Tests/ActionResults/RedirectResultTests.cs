using AtleX.Web.Mvc.ActionResults;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AtleX.Web.Mvc.Tests.ActionResults
{
    [TestFixture]
    class RedirectResultTests
    {
        [Test]
        public void PermanentRedirectSetsPermanentFlag_Successful()
        {
            PermanentRedirect r = new PermanentRedirect("http://localhost/");

            Assert.IsTrue(r.Permanent);
        }

        [Test]
        public void PermanentRedirectIsActionResult_Successful()
        {
            PermanentRedirect r = new PermanentRedirect("http://localhost/");

            Assert.IsInstanceOf<ActionResult>(r);
        }

        [Test]
        public void TemporaryRedirectDoesNotSetPermanentFlag_Successful()
        {
            TemporaryRedirect r = new TemporaryRedirect("http://localhost/");

            Assert.IsFalse(r.Permanent);
        }

        [Test]
        public void TemporaryRedirectIsActionResult_Successful()
        {
            TemporaryRedirect r = new TemporaryRedirect("http://localhost/");

            Assert.IsInstanceOf<ActionResult>(r);
        }
    }
}
