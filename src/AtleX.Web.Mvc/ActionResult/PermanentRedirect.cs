using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtleX.Web.Mvc.ActionResult
{
    public class PermanentRedirect : RedirectResultBase
    {
        public PermanentRedirect(Uri location)
            : this(location.ToString())
        {
        }

        public PermanentRedirect(string location)
        {
            this.location = location;
            this.httpStatusCode = 301;
        }
    }
}
