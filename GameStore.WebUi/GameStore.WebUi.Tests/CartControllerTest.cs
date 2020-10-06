using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using GameStore.WebUi.Models;
using GameStore.WebUI.HtmlHelpers;
using GameStore.WebUI.Models;
using GameStore.WebUi.Controllers;


namespace GameStore.UnitTests
{
    [TestClass]
    class CartControllerTest
    {

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
        new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
    }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }

        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(g => g.Games).Returns(new List<Game> { new Game { GameId = 1, Name = "Игра1", Category = "Кат1" } }.AsQueryable());


            Cart cart = new Cart();

            CartController cartController = new CartController(mock.Object);


            RedirectToRouteResult res = cartController.AddToCart(cart, 2, "MyURL");


            Assert.AreEqual(res.RouteValues["action"], "Index");
            Assert.AreEqual(res.RouteValues["returnUrl"], "myUrl");

        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}
