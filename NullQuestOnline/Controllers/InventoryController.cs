using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using NullQuest.Game.Combat;
using NullQuestOnline.Data;
using NullQuestOnline.Extensions;
using NullQuestOnline.Models;

namespace NullQuestOnline.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IAccountRepository accountRepository;

        public InventoryController()
        {
            accountRepository = new AccountRepository();
        }

        public ActionResult Index()
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            return View(InventoryViewModel.Create(world));
        }

        [HttpPost]
        public ActionResult Change(string equip, string destroy)
        {
            var world = accountRepository.LoadWorld(User.Identity.Name);
            if (equip != null)
            {
                var weapon = world.Character.Inventory.Single(x => x.Id == new Guid(equip));

                if (!world.Character.Weapon.Equals(Weapon.BareHands))
                {
                    world.Character.AddItemToInventory(world.Character.Weapon);
                }
                
                world.Character.RemoveItemFromInventory(weapon);
                world.Character.Weapon = weapon.DeepClone();
                world.Character.Weapon.Quantity = 1;
                accountRepository.SaveWorld(world);
            }

            if (destroy != null)
            {
                var weapon = world.Character.Inventory.Single(x => x.Id == new Guid(destroy));
                world.Character.RemoveItemFromInventory(weapon);
                accountRepository.SaveWorld(world);
            }
            return View("Index", InventoryViewModel.Create(world));
        }
    }
}