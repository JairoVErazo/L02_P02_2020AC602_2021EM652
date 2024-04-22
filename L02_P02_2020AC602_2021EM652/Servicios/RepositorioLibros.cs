using L02_P02_2020AC602_2021EM652.Models;
using Microsoft.EntityFrameworkCore;

namespace L02_P02_2020AC602_2021EM652.Servicios
{
    public interface IRepositorioLibros
    {
        Task<IEnumerable<Libro>> ObtenerLibrosConAutor();
    }
    public class RepositorioLibros : IRepositorioLibros
    {
        private readonly LibreriaContext _context;

        public RepositorioLibros(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Libro>> ObtenerLibrosConAutor()
        {
            var librosConAutor = await _context.Libros.Include(x => x.IdAutorNavigation).ToListAsync();

            return librosConAutor;
        }
    }
}
