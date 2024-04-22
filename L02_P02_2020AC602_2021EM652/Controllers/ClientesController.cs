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

            await repositorioClientes.CrearCliente(model);

            return RedirectToAction("Listado");
        }

        public async Task<IActionResult> Listado()
        {
            int idCliente = await repositorioClientes.UltimoCliente();

            var listado = await repositorioLibros.ObtenerLibrosConAutor();

            var librosSeleccionados = await repositorioClientes.ObtenerDetalleDeVenta(idCliente);

            var totalPrecio = librosSeleccionados.Where(x => x.IdLibroNavigation != null)
                                         .Sum(x => x.IdLibroNavigation.Precio ?? 0);

            ViewBag.listaSeleccionados = new
            {
                libros = librosSeleccionados.Count(),
                total = totalPrecio
            };

            return View(listado);
        }

        public IActionResult AgregarACarrito() 
        {
            return RedirectToAction("Listado");
        }

        [HttpPost]
        public async  Task<IActionResult> AgregarACarrito(int libro)
        {
            int cliente = await repositorioClientes.UltimoCliente();

            int idPedido = await repositorioClientes.ObtenerPedido(cliente);

            await repositorioClientes.AgregarLibroAlDetalle(libro, idPedido);

            return RedirectToAction("Listado");
        }

        public async Task<IActionResult> Cierre()
        {
            int idCliente = await repositorioClientes.UltimoCliente();

            var cliente = await repositorioClientes.ObtenerCliente(idCliente);

            var librosDetalle = await repositorioClientes.ObtenerDetalleDeVenta(idCliente);

            var totalPrecio = librosDetalle.Where(x => x.IdLibroNavigation != null)
                                        .Sum(x => x.IdLibroNavigation.Precio ?? 0);

            var cierreDeCliente = new CierreViewModel()
            {
                Cliente = cliente,
                PedidoDetalles = librosDetalle,
                PrecioTotal = totalPrecio
            };

            if (ViewBag.Exito == null)
            {
                ViewBag.Exito = false; 
            }

            return View(cierreDeCliente);
        }

        [HttpPost]
        public async Task<IActionResult> Cierre( int idCliente, int libros, decimal total)
        {

            await repositorioClientes.ActualizarPedido(idCliente, libros, total);

            bool actualizacionExitosa = true;


           ViewBag.Exito = actualizacionExitosa;

            return RedirectToAction("Index");
            
        }
    }
}
