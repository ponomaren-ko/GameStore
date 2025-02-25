﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using GameStore.WebUi.Controllers;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(x => x.Games).Returns(new List<Game>
            {
                 new Game { GameId = 1, Name = "Игра1"},
                new Game { GameId = 2, Name = "Игра2"},
                new Game { GameId = 3, Name = "Игра3"},
                new Game { GameId = 4, Name = "Игра4"},
                new Game { GameId = 5, Name = "Игра5"}
            });

            AdminController controller = new AdminController(mock.Object);

            List<Game> result = ((IEnumerable<Game>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count,5);
            Assert.AreEqual("Игра1", result[0].Name);
            Assert.AreEqual("Игра2", result[1].Name);
            Assert.AreEqual("Игра3", result[2].Name);
        }

        [TestMethod]

        public void Can_Edit_Game()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                   new Game { GameId = 1, Name = "Игра1"},
                new Game { GameId = 2, Name = "Игра2"},
                new Game { GameId = 3, Name = "Игра3"},
                new Game { GameId = 4, Name = "Игра4"},
                new Game { GameId = 5, Name = "Игра5"}
            });

            AdminController controller = new AdminController(mock.Object);

            Game game1 = controller.Edit(1).Model as Game;
            Game game2 = controller.Edit(2).Model as Game;
            Game game3 = controller.Edit(3).Model as Game;

            Assert.AreEqual(1,game1.GameId);
            Assert.AreEqual(2,game2.GameId);
            Assert.AreEqual(3,game3.GameId);
           
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
           
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Игра1"},
        new Game { GameId = 2, Name = "Игра2"},
        new Game { GameId = 3, Name = "Игра3"},
        new Game { GameId = 4, Name = "Игра4"},
        new Game { GameId = 5, Name = "Игра5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Game result = controller.Edit(6).ViewData.Model as Game;

            // Assert
        }

        [TestMethod]

        public void Can_Save_Valid_Changes()
        {

            Mock<IGameRepository> mock = new Mock<IGameRepository>();

            AdminController controller = new AdminController(mock.Object);

            Game game = new Game { Name = "game1" };

            ActionResult result = controller.Edit(game);
            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveGame(game));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));


        }
        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
         
            Mock<IGameRepository> mock = new Mock<IGameRepository>();

          
            AdminController controller = new AdminController(mock.Object);

           
            Game game = new Game { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(game);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            // Организация - создание объекта Game
            Game game = new Game { GameId = 2, Name = "Игра2" };

            // Организация - создание имитированного хранилища данных
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Игра1"},
        new Game { GameId = 2, Name = "Игра2"},
        new Game { GameId = 3, Name = "Игра3"},
        new Game { GameId = 4, Name = "Игра4"},
        new Game { GameId = 5, Name = "Игра5"}
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(game.GameId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeleteGame(game.GameId));
        }
    }
}