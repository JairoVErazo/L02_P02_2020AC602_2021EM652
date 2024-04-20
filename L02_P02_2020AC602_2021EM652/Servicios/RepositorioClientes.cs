using L02_P02_2020AC602_2021EM652.Models;

namespace L02_P02_2020AC602_2021EM652.Servicios
{

    public interface IRepositorioClientes
    {
        Task<int> CrearCliente(Cliente cliente);
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

            return id;


        }
    }
}
