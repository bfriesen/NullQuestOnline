using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NullQuestOnline.Data;
using NullQuestOnline.Game;
using NullQuestOnline.Models.ViewModels;

namespace NullQuestOnline.Controllers
{
    public class DungeonController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public DungeonController()
        {
            accountRepository = new AccountRepository();
        }

        public ActionResult Index()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (!world.InDungeon)
            {
                world.InDungeon = true;
                accountRepository.SaveCharacter(world);
            }

            var m = new Monster() {Name = "Baddie"};
            m.CurrentHitPoints = Dice.Random(0, m.MaxHitPoints);
            return View(new DungeonViewModel()
            {
                GameWorld = world,
                DungeonName = world.GetCurrentDungeonName(),
                DungeonLevel = world.CurrentDungeonLevel,
                FluffText = "This is the dungeon - it isn't a very friendly place. Hallways go off in seemingly every direction, with no end in sight. A skeleton sits upright against a wall in the corner. No doubt an adventurer that came before you...",
                Monster = m // world.CurrentEncounter
            });
        }

        public ActionResult GoDeeper()
        {
            throw new NotImplementedException();
        }
    }
}
