using GameStore.WebUi.Infrastructure.Abstract;
using GameStore.WebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUi.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider provider)
        {
            authProvider = provider;
        }

       

        public ViewResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
                return View();
        }

    }
}