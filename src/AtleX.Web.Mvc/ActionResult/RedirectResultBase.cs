using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AtleX.Web.Mvc.ActionResult
{
    public abstract class RedirectResultBase : System.Web.Mvc.ActionResult
    {
        protected int httpStatusCode;
        protected string location;

        public override void ExecuteResult(System.Web.Mvc.ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;

            response.StatusCode = httpStatusCode;
            response.Headers.Add("Location", location);
        }
    }
}
