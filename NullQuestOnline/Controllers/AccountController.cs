using System.Web.Mvc;
using System.Web.Security;
using NullQuestOnline.Models;

namespace NullQuestOnline.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Character character, string returnUrl)
        {
            FormsAuthentication.SetAuthCookie(character.Name, true);

            if (returnUrl == null)
            {
                return RedirectToAction("Index", "Town");
            }

            return Redirect(returnUrl);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
