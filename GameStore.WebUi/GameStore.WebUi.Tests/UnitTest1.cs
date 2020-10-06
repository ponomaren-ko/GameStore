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
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Игра1"},
        new Game { GameId = 2, Name = "Игра2"},
        new Game { GameId = 3, Name = "Игра3"},
        new Game { GameId = 4, Name = "Игра4"},
        new Game { GameId = 5, Name = "Игра5"}
    });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
    {
        new Game { GameId = 1, Name = "Игра1"},
        new Game { GameId = 2, Name = "Игра2"},
        new Game { GameId = 3, Name = "Игра3"},
        new Game { GameId = 4, Name = "Игра4"},
        new Game { GameId = 5, Name = "Игра5"}
    });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Act
            GamesListViewModel result
                = (GamesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]

        public void Can_Filter_Games()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
                         {
                             new Game { GameId = 1, Name = "Игра1", Category = "Cat1" },
                             new Game { GameId = 2, Name = "Игра2", Category = "Cat1" },
                             new Game { GameId = 3, Name = "Игра3", Category = "Cat3" },
                             new Game { GameId = 4, Name = "Игра4", Category = "Cat4" },
                             new Game { GameId = 5, Name = "Игра5", Category = "Cat5" }

                         }
            );
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;



            List<Game> result = ((GamesListViewModel)controller.List("Cat1", 1).Model).Games.ToList();




            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Name, "Игра1");
            Assert.AreEqual(result[1].Name, "Игра2");


        }

        [TestMethod]

        public void Can_Create_Categories()
        {
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(
                new List<Game>
                {
                    new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
                     new Game { GameId = 2, Name = "Игра2", Category="Симулятор"},
                     new Game { GameId = 3, Name = "Игра3", Category="Шутер"},
                     new Game { GameId = 4, Name = "Игра4", Category="RPG"}
                });

            NavController controller = new NavController(mock.Object);

            List<string> result = ((IEnumerable<string>)controller.Menu().Model).ToList();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "RPG");
            Assert.AreEqual(result[1], "Шутер");
            Assert.AreEqual(result[2], "Симулятор");

        }


        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
            {
        new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
        new Game { GameId = 2, Name = "Игра2", Category="Шутер"}
             });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Шутер";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
                new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
                new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
                new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
                new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}