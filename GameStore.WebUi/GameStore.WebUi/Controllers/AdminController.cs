using GameStore.Domain.Abstract;
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
    }
}