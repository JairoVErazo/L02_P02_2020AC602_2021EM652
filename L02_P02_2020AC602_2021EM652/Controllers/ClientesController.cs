using Microsoft.AspNetCore.Mvc;

namespace L02_P02_2020AC602_2021EM652.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Listado()
        {
            return View();
        }

        public IActionResult Cierre()
        {
            return View();
        }
    }
}
