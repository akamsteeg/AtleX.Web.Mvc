using AtleX.Web.Mvc.ActionResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AtleX.Web.Mvc
{
    public static class ControllerExtensions
    {
        public static System.Web.Mvc.ActionResult HttpRedirectPermanent(this Controller controller, string location)
        {
            return new PermanentRedirect(location);
        }

        public static System.Web.Mvc.ActionResult HttpRedirectPermanent(this Controller controller, Uri location)
        {
            return controller.HttpRedirectPermanent(location);
        }

        public static System.Web.Mvc.ActionResult HttpRedirectTemporary(this Controller controller, string location)
        {
            return new PermanentRedirect(location);
        }

        public static System.Web.Mvc.ActionResult HttpRedirectTemporary(this Controller controller, Uri location)
        {
            return controller.HttpRedirectTemporary(location);
        }
    }
}
