namespace NextStepMVC.Controllers
{
  using System;
  using System.Net;
  using System.Web.Mvc;

  public class ErrorsController : Controller
  {
    public ActionResult Error404()
    {
      this.Response.StatusCode = (int)HttpStatusCode.NotFound;
      return this.View();
    }

    public ActionResult Error500()
    {
      this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      return this.View();
    }

      public ActionResult UnknownDomain()
      {
          this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return this.View();
      }

    public ActionResult Crash()
    {
      var i = 1;
      if (i == 2)
      {
        return this.View();
      }

      throw new NotImplementedException();
    }
  }
}