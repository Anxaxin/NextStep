namespace NextStepMVC
{
  using System;
  using System.Net;
  using System.Threading;
  using System.Web;
  using System.Web.Mvc;
  using System.Web.Optimization;
  using System.Web.Routing;
  using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
  using Microsoft.Practices.EnterpriseLibrary.Logging;
  using Sima.Common.Web.EntLib.Logging;

  public class Global : HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      EnterpriseLibraryContainer.Current = new DefaultWebsiteConfiguration().CreateContainer("SimaNextStep");

      AreaRegistration.RegisterAllAreas();
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(new RazorViewEngine());
    }

    protected void Session_Start(object sender, EventArgs e)
    {
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
      var currentDomain = Settings.GetCurrentDomain();
      if (currentDomain == null)
      {
        HttpContext.Current.RewritePath("/Errors/UnknowDomain");
      }
      else
      {
        if (currentDomain.RedirectToMaster && !currentDomain.NextStepUrl)
        {
          this.Response.Cache.SetExpires(DateTime.Now.AddHours(-1));
          this.Response.Cache.SetNoStore();
          var ub = new UriBuilder(this.Request.Url);
          ub.Host = currentDomain.MasterUrl;
          var u = ub.Uri.ToString();
          this.Response.RedirectPermanent(u, true);
        }
        else
        {
          Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture = Settings.GetCurrentCulture();
        }
      }
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      var exception = Server.GetLastError();
      var ex2 = exception as HttpException;
      try
      {
        if (ex2 == null || ex2.GetHttpCode() != (int)HttpStatusCode.NotFound)
        {
          DefaultWebsiteConfiguration.LogException(this.Request, exception);
        }
      }
      catch
      {
        Logger.Write(new LogEntry(exception.Message, "Generel", 0, 0, System.Diagnostics.TraceEventType.Error, "MvcApplication.Application_Error", null));
        throw;
      }
    }

    protected void Session_End(object sender, EventArgs e)
    {
    }

    protected void Application_End(object sender, EventArgs e)
    {
    }
  }
}