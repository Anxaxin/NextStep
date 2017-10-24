namespace NextStepMVC.Controllers
{
    using System.Web.Mvc;
    using Models;

    public class HomeController : Controller
    {
        private readonly RetrieveContent rc;

        public HomeController()
        {
            this.rc = new RetrieveContent();
            this.rc.GetIndexing();
        }

        public ActionResult Index()
        {
            return this.View(this.rc);
        }

        public ActionResult Panel(string mainId, string subId)
        {
            this.ViewBag.mainId = mainId;
            this.ViewBag.subId = subId;
            return this.View(this.rc);
        }
    }
    //
}