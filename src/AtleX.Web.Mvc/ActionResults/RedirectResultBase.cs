using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AtleX.Web.Mvc.ActionResults
{
    public abstract class RedirectResultBase : RedirectResult
    {
        public RedirectResultBase(string url, bool permanent)
            : base(url, permanent)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;

            response.StatusCode = (this.Permanent) ? 301 : 302;
            response.Headers.Add("Location", this.Url);
        }
    }
}
