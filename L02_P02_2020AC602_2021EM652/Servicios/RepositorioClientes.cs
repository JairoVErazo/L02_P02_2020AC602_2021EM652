using L02_P02_2020AC602_2021EM652.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace L02_P02_2020AC602_2021EM652.Servicios
{

    public interface IRepositorioClientes
    {
        Task<PedidoEncabezado> ActualizarPedido(int cliente, int libros, decimal total);
        Task <int>AgregarLibroAlDetalle(int libro, int pedido);
        Task<int> CrearCliente(Cliente cliente);
        Task<Cliente> ObtenerCliente(int idCliente);
        Task<IEnumerable<PedidoDetalle>> ObtenerDetalleDeVenta(int idCliente);
        Task<int> ObtenerPedido(int cliente);
        Task<int> UltimoCliente();
    }
    public class RepositorioClientes : IRepositorioClientes
    {
        private readonly LibreriaContext _context;

        public RepositorioClientes(LibreriaContext contextDb)
        {
            _context = contextDb;
        }


        public async Task<int> CrearCliente(Cliente cliente)
        {
            if(cliente is null) ArgumentNullException.ThrowIfNull(cliente);

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


        public async Task<int>AgregarLibroAlDetalle(int libroId,int pedidoId)
        {
            var libro = await _context.Libros.FindAsync(libroId);
            var pedido = await _context.PedidoEncabezados.FindAsync(pedidoId);

            PedidoDetalle libroAgregado = new()
            {
                IdLibro = libroId,
                IdPedido = pedidoId,
                IdLibroNavigation = libro,
                IdPedidoNavigation = pedido,
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

        public async Task<Cliente> ObtenerCliente(int idCliente)
        {
            Cliente? cliente = await _context.Clientes.Where(x => x.Id == idCliente).SingleOrDefaultAsync();
            return cliente;
        }

        public async Task<int> UltimoCliente()
        {
            var cliente = await _context.Clientes.OrderByDescending(x => x).FirstOrDefaultAsync();

            int idCliente = cliente.Id;

            return idCliente;
        }

        public async Task<IEnumerable<PedidoDetalle>> ObtenerDetalleDeVenta(int idCliente)
        {
            var cliente = await ObtenerCliente(idCliente);
            var pedido = await _context.PedidoEncabezados.Where(x => x.IdCliente == cliente.Id).FirstOrDefaultAsync();

            var librosDePedido = await _context.PedidoDetalles
                                .Where(c => c.IdPedido == pedido.Id)
                                .Include(x => x.IdLibroNavigation)
                                .Include(y => y.IdLibroNavigation.IdAutorNavigation)
                                .ToListAsync();
            return librosDePedido;
        }

        public async Task<PedidoEncabezado> ActualizarPedido( int cliente, int libros, decimal total)
        {
            
            var pedido = await _context.PedidoEncabezados.Where(x => x.IdCliente == cliente).FirstOrDefaultAsync();

            pedido.Estado = "C";
            pedido.CantidadLibros = libros;
            pedido.Total = total;

            _context.PedidoEncabezados.Update(pedido);
            await _context.SaveChangesAsync();

            return pedido;

        }
    }
}
