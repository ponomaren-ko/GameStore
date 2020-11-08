using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.WebUi.Infrastructure.Abstract;
using GameStore.WebUi.Models;
using GameStore.WebUi.Controllers;

namespace GameStore.WebUi.Tests
{
    [TestClass]
    public class AdminSecurityTests
    {

        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);

            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "12345"
            };

            AccountController controller = new AccountController(mock.Object);

            ActionResult result = controller.Login(model,"/MyURL");



            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);



        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin","12345")).Returns(true);

             AccountController controller = new AccountController(mock.Object);

            LoginViewModel loginViewModel = new LoginViewModel
            {
                UserName = "xxxxx",
                Password = "incorrectPass"
            };

            ActionResult result = controller.Login(loginViewModel, "/MyURL");


            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);


        }
    }
}
