using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NullQuestOnline.Controllers
{
    [Authorize]
    public class TownController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tavern()
        {
            return View();
        }
    }
}
