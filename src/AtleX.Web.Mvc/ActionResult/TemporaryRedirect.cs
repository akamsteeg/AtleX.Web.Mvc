using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AtleX.Web.Mvc.ActionResult
{
    public class TemporaryRedirect : RedirectResultBase
    {
        public TemporaryRedirect(Uri location)
            : this(location.ToString())
        {
        }

        public TemporaryRedirect(string location)
            : base(location, false)
        {
        }
    }
}
