using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NullQuestOnline.Data;

namespace NullQuestOnline.Controllers
{
    [Authorize]
    public class TownController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public TownController()
        {
            accountRepository = new AccountRepository();
        }

        public ActionResult Index()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (!world.Character.IsAlive)
            {
                world.Character.RestoreHealth(world.Character.MaxHitPoints);
                world.CurrentEncounter = null;
            }
            world.NumberOfMonstersDefeatedInCurrentDungeonLevel = 0;
            accountRepository.SaveCharacter(world);

            return View();
        }

        public ActionResult Tavern()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            world.Character.RestoreHealth(world.Character.MaxHitPoints);
            world.NumberOfMonstersDefeatedInCurrentDungeonLevel = 0;
            accountRepository.SaveCharacter(world);

            return View();
        }
    }
}
