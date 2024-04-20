using L02_P02_2020AC602_2021EM652.Models;
using L02_P02_2020AC602_2021EM652.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace L02_P02_2020AC602_2021EM652.Controllers
{
    public class ClientesController : Controller
    {
        public ClientesController(IRepositorioClientes repositorioClientes)
        {
            RepositorioClientes = repositorioClientes;
        }

        public IRepositorioClientes RepositorioClientes { get; }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(Cliente model)
        {
           if(!ModelState.IsValid)
            {
                return View(model);
            }

            var cliente = await RepositorioClientes.CrearCliente(model);

            return RedirectToAction("Listado");
        }

        public IActionResult Listado()
        {
            return View();
        }
    }
}
