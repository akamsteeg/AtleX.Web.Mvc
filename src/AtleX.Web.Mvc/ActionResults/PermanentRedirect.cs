using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AtleX.Web.Mvc.ActionResults
{
    public class PermanentRedirect : RedirectResultBase
    {
        public PermanentRedirect(Uri location)
            : this(location.ToString())
        {
        }

        public PermanentRedirect(string location)
            : base(location, true)
        {
        }
    }
}
