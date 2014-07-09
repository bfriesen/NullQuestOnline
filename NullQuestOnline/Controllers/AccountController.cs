using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NullQuestOnline.Data;
using NullQuestOnline.Game;
using NullQuestOnline.Models;

namespace NullQuestOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public AccountController()
        {
            accountRepository = new AccountRepository();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string characterName, string returnUrl)
        {
            if (accountRepository.IsCharacterCreated(characterName))
            {
                FormsAuthentication.SetAuthCookie(characterName, true);

                if (returnUrl == null)
                {
                    return RedirectToAction("Index", "Town");
                }

                return Redirect(returnUrl);
            }

            return RedirectToAction("Create", new { characterName, returnUrl });
        }

        public ActionResult Create(string characterName)
        {
            var newCharacter = new Character
            {
                Name = characterName,
                
                CharacterPowerRoll = 9 + Dice.Roll(4, 4)
            };

            accountRepository.SaveCharacter(newCharacter);

            return View(newCharacter);
        }

        [HttpPost]
        public ActionResult Create(string command, string characterName, string returnUrl)
        {
            switch (command)
            {
                case "accept":
                    var character = accountRepository.LoadCharacter(characterName);
                    if (!character.Created)
                    {
                        character.Created = true;
                        character.CurrentHitPoints = character.MaxHitPoints;
                        accountRepository.SaveCharacter(character);
                    }
                    FormsAuthentication.SetAuthCookie(characterName, true);
                    return RedirectToAction("Index", "Town");
                case "reroll":
                    return RedirectToAction("Create", new { characterName, returnUrl });
            }
            return RedirectToAction("Create", new { characterName, returnUrl });
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
