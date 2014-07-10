using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NullQuest.Game.Combat;
using NullQuestOnline.Data;
using NullQuestOnline.Game;
using NullQuestOnline.Game.Combat;
using NullQuestOnline.Game.Factories;
using NullQuestOnline.Models;

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
                world.CombatLog = new List<string>();
                accountRepository.SaveWorld(world);
            }
            if (world.InDungeon && world.CurrentEncounter != null && (!world.CurrentEncounter.IsAlive || world.Character.HasFledCombat))
            {
                world.CurrentEncounter = null;
                world.Character.HasFledCombat = false;
                world.CombatLog = new List<string>();
                accountRepository.SaveWorld(world);
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
                    world.CombatLog = new List<string>();
                    accountRepository.SaveWorld(world);
                }
                return View("Index", DungeonViewModel.Create(world));
            }
            return RedirectToAction("Index");
        }

        public ActionResult Attack()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (world.InDungeon)
            {
                if (world.CurrentEncounter != null)
                {
                    if (world.CurrentEncounter.IsAlive && world.Character.IsAlive)
                    {
                        CombatCalculator.Attack(world.Character, world.CurrentEncounter, world.CombatLog);
                        if (world.CurrentEncounter.IsAlive)
                        {
                            CombatCalculator.Attack(world.CurrentEncounter, world.Character, world.CombatLog);
                        }

                        world.CombatLog.Add("");

                        if (!world.Character.IsAlive)
                        {
                            world.CombatLog.Add(string.Format("Dude, the {0} totally killed you! Bummer.",
                                world.CurrentEncounter.Name));
                        }

                        if (!world.CurrentEncounter.IsAlive)
                        {
                            world.CombatLog.Add(string.Format(
                                "Dude, you totally killed that {0}! Awesome.{1}You received {2} XP and {3} gold.",
                                world.CurrentEncounter.Name,
                                "<br />",
                                world.CurrentEncounter.Experience,
                                world.CurrentEncounter.Gold));

                            world.Character.Gold += world.CurrentEncounter.Gold;
                            world.Character.AddExperience(world.CurrentEncounter.Experience);

                            if (world.CurrentEncounter.Weapon != null &&
                                !world.CurrentEncounter.Weapon.Equals(Weapon.BareHands))
                            {
                                world.CombatLog.Add(string.Format("&nbsp;-&nbsp;{0} dropped {1}",
                                    world.CurrentEncounter.Name, world.CurrentEncounter.Weapon.Name));
                                world.Character.AddItemToInventory(world.CurrentEncounter.Weapon);
                                world.CurrentEncounter.Weapon = null;
                            }

                            if (world.CurrentEncounter.IsBoss)
                            {
                                world.CurrentDungeonLevel++;
                                world.SetRequiredNumberOfMonstersInCurrentDungeonLevelBeforeBoss();
                            }
                            else
                            {
                                world.NumberOfMonstersDefeatedInCurrentDungeonLevel++;
                            }
                        }
                    }

                    accountRepository.SaveWorld(world);
                    return View("Index", DungeonViewModel.Create(world));
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Flee()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (world.InDungeon)
            {
                if (world.CurrentEncounter != null)
                {
                    if (world.CurrentEncounter.IsAlive && world.Character.IsAlive)
                    {
                        CombatCalculator.Flee(world.Character, world.CurrentEncounter, world.CombatLog);
                        if (!world.Character.HasFledCombat)
                        {
                            CombatCalculator.Attack(world.CurrentEncounter, world.Character, world.CombatLog);
                        }

                        world.CombatLog.Add("");

                        if (!world.Character.IsAlive)
                        {
                            world.CombatLog.Add(string.Format("Dude, the {0} totally killed you! Bummer.",
                                world.CurrentEncounter.Name));
                        }

                        accountRepository.SaveWorld(world);
                        return View("Index", DungeonViewModel.Create(world));
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
