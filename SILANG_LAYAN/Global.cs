using System;
using System.Web;

namespace SILANG_LAYAN

{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                MyApplication.InitApplication();
            }
            catch
            {
                base.Server.ClearError();
                base.Response.Clear();
                base.Response.Redirect(MyApplication.LoginPage);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Uri url = HttpContext.Current.Request.Url;
            if (HttpContext.Current.Request.Browser.Browser.ToLower().Contains("chrome") && url.AbsolutePath.ToLower().Contains("reserved.reportviewerwebcontrol.axd") && !url.Query.ToLower().Contains("iterationid"))
            {
                HttpContext.Current.RewritePath(url.PathAndQuery + "&IterationId=0");
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }

}


