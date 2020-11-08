using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.WebUi.Controllers
{
    public class AdminController : Controller
    {
        IGameRepository repository;

        public AdminController(IGameRepository repository)
        {
            this.repository = repository;

        }

        // GET: Admin
        public  ViewResult Index()
        {
            return View(repository.Games);
        }
        public ViewResult Edit(int gameId)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            return View(game);
        }

        [HttpPost]
        public ActionResult Edit(Game game)
        {
            if (ModelState.IsValid)
            {
                repository.SaveGame(game);
                TempData["message"] = string.Format("Изменения ы игре \"{0}\" были сохранены", game.Name );
                return RedirectToAction("Index");

            }
            else
            {
                return View(game);
            }

        }
        public ViewResult Create()
        {
            return View("Edit", new Game());
        }

        [HttpPost]
        public ActionResult Delete(int gameId)
        {
            Game deletedGame = repository.DeleteGame(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    deletedGame.Name);
            }
            return RedirectToAction("Index");
        }
    }
}