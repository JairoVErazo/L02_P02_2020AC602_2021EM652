using L02_P02_2020AC602_2021EM652.Models;
using L02_P02_2020AC602_2021EM652.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace L02_P02_2020AC602_2021EM652.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IRepositorioLibros repositorioLibros;

        public ClientesController(IRepositorioClientes repositorioClientes,
                                  IRepositorioLibros repositorioLibros)
        {
            this.repositorioClientes = repositorioClientes;
            this.repositorioLibros = repositorioLibros;
        }

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

            var cliente = await repositorioClientes.CrearCliente(model);

            TempData["Cliente"] = cliente;

            return RedirectToAction("Listado");
        }

        public async Task<IActionResult> Listado()
        {

            var listado = await repositorioLibros.ObtenerLibrosConAutor();

            return View(listado);
        }

        public async  Task<IActionResult> AgregarACarrito(int libro)
        {
            int cliente = (int)TempData["Cliente"];

            int idPedido = await repositorioClientes.ObtenerPedido(cliente);

            await repositorioClientes.AgregarLibroAlDetalle(libro, idPedido);

            return RedirectToAction("Cierre");
        }

        public IActionResult Cierre()
        {
            return View();
        }
    }
}
