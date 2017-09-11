using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoManager.Models
{
    public class ConnectedUserOnly : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["CurrentUser"] != null)
                    return true;
                else
                    httpContext.Response.Redirect("/home");

            return base.AuthorizeCore(httpContext);
        }
    }
}