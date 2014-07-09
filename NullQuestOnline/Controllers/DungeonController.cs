using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NullQuestOnline.Data;
using NullQuestOnline.Game;
using NullQuestOnline.Game.Factories;
using NullQuestOnline.Models.ViewModels;

namespace NullQuestOnline.Controllers
{
    public class DungeonController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly MonsterFactory monsterFactory;

        public DungeonController()
        {
            accountRepository = new AccountRepository();
            monsterFactory = new MonsterFactory();
        }

        public ActionResult Index()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (!world.InDungeon)
            {
                world.InDungeon = true;
                world.SetRequiredNumberOfMonstersInCurrentDungeonLevelBeforeBoss();
                accountRepository.SaveCharacter(world);
            }

            return View(DungeonViewModel.Create(world));
        }

        public ActionResult GoDeeper()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (world.InDungeon)
            {
                if (world.CurrentEncounter == null)
                {
                    world.CurrentEncounter = monsterFactory.CreateMonster(world);
                    accountRepository.SaveCharacter(world);
                }
                return View("Index", DungeonViewModel.Create(world));
            }
            return RedirectToAction("Index");
        }

        public ActionResult Attack()
        {
            return null;
        }

        public ActionResult Flee()
        {
            return null;
        }
    }
}
