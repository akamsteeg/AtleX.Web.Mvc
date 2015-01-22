﻿using AtleX.Web.Mvc.ActionResult;
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
        /// <summary>
        /// Does a HTTP/301 Redirect Permanently without
        /// output HTML
        /// </summary>
        /// <param name="location">The URL to redirect to</param>
        /// <returns></returns>
        public static System.Web.Mvc.ActionResult HttpRedirectPermanent(this Controller controller, string location)
        {
            return new PermanentRedirect(location);
        }

        /// <summary>
        /// Does a HTTP/301 Redirect Permanently without
        /// output HTML
        /// </summary>
        /// <param name="location">The URL to redirect to</param>
        /// <returns></returns>
        public static System.Web.Mvc.ActionResult HttpRedirectPermanent(this Controller controller, Uri location)
        {
            return controller.HttpRedirectPermanent(location);
        }

        /// <summary>
        /// Does a HTTP/302 Temporary Redirect without
        /// output HTML
        /// </summary>
        /// <param name="location">The URL to redirect to</param>
        /// <returns></returns>
        public static System.Web.Mvc.ActionResult HttpRedirectTemporary(this Controller controller, string location)
        {
            return new PermanentRedirect(location);
        }

        /// <summary>
        /// Does a HTTP/302 Temporary Redirect without
        /// output HTML
        /// </summary>
        /// <param name="location">The URL to redirect to</param>
        /// <returns></returns>
        public static System.Web.Mvc.ActionResult HttpRedirectTemporary(this Controller controller, Uri location)
        {
            return controller.HttpRedirectTemporary(location);
        }
    }
}
