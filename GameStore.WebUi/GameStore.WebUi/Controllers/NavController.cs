﻿using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUi.Controllers
{
    public class NavController : Controller
    {

        IGameRepository repository;


        public NavController(IGameRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Games
                .Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);

            return PartialView("FlexMenu", categories);
        }
    }
}