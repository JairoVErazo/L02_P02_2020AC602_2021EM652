using L02_P02_2020AC602_2021EM652.Models;
using Microsoft.EntityFrameworkCore;

namespace L02_P02_2020AC602_2021EM652.Servicios
{

    public interface IRepositorioClientes
    {
        Task <int>AgregarLibroAlDetalle(int libro, int pedido);
        Task<int> CrearCliente(Cliente cliente);
        Task<int> ObtenerPedido(int cliente);
    }
    public class RepositorioClientes : IRepositorioClientes
    {
        private readonly LibreriaDbContext _context;

        public RepositorioClientes(LibreriaDbContext contextDb)
        {
            _context = contextDb;
        }


        public async Task<int> CrearCliente(Cliente cliente)
        {
            if(cliente == null) throw new ArgumentNullException(nameof(cliente));

            DateTime hoy = DateTime.Now;

            Cliente nuevoCliente = new()
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                Direccion = cliente.Direccion,
                CreatedAt = hoy,
            };

            await _context.AddAsync(nuevoCliente);
            await _context.SaveChangesAsync();

            int id = nuevoCliente.Id;

            await CrearPedido(id);

            return id;


        }

        public async Task CrearPedido(int cliente)
        {
            PedidoEncabezado nuevoPedido = new()
            {
                Estado = "P",
                IdCliente = cliente,
            };


            await _context.PedidoEncabezados.AddAsync(nuevoPedido);
            await _context.SaveChangesAsync();
        }


        public async Task<int>AgregarLibroAlDetalle(int libro,int pedido)
        {
            PedidoDetalle libroAgregado = new()
            {
                IdLibro = libro,
                IdPedido = pedido,
            };

            await _context.PedidoDetalles.AddAsync(libroAgregado);
            await _context.SaveChangesAsync();

            int idDetalle = libroAgregado.Id;

            return idDetalle;
        }

        public async Task<int> ObtenerPedido(int cliente)
        {
            var pedido = await _context.PedidoEncabezados.Where(x => x.IdCliente == cliente).OrderByDescending(X => X.Id).FirstOrDefaultAsync();

            int idPedido = pedido.Id;

            return idPedido;
        }

    }
}
