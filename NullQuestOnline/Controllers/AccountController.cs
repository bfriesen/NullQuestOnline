﻿using System.Web.Mvc;
using NullQuestOnline.Data;
using NullQuestOnline.Game;
using NullQuestOnline.Helpers;

namespace NullQuestOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository accountRepository;
        private readonly AuthHelper authHelper;

        public AccountController()
        {
            accountRepository = new AccountRepository();
            authHelper = new AuthHelper();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string characterName, string returnUrl)
        {
            if (accountRepository.IsCharacterCreated(characterName))
            {
                authHelper.SignIn(characterName);

                if (returnUrl == null)
                {
                    return RedirectToAction("Index", "Town");
                }

                return Redirect(returnUrl);
            }

            return RedirectToAction("Create", new { characterName });
        }

        [HttpGet]
        public ActionResult Create(string characterName)
        {
            GameWorld newWorld = GameWorld.Create(characterName);

            accountRepository.SaveWorld(newWorld);

            return View(newWorld.Character);
        }

        [HttpPost]
        public ActionResult Create(string command, string characterName)
        {
            switch (command)
            {
                case "accept":
                    GameWorld world = accountRepository.LoadWorld(characterName);

                    if (!world.IsAccepted)
                    {
                        world.Accept();
                        accountRepository.SaveWorld(world);
                    }

                    authHelper.SignIn(characterName);

                    return RedirectToAction("Index", "Town");

                case "reroll":
                    return RedirectToAction("Create", new { characterName });
            }

            return RedirectToAction("Create", new { characterName });
        }

        [HttpPost]
        public ActionResult Logout()
        {
            authHelper.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
