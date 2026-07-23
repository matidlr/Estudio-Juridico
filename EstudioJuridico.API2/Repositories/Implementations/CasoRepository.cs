using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstudioJuridico.API2.Repositories.Implementations
{
    public class CasoRepository : BaseRepository<Caso>, ICasoRepository
    {
        public CasoRepository(AppDbContext db) : base(db) { }

        public async Task<List<Caso>> GetTodosAsync()
        {
            return await _db.Casos
                .Include(c => c.Cliente).ThenInclude(cl => cl.Usuario)
                .Include(c => c.Abogado).ThenInclude(a => a.Usuario)
                .OrderByDescending(c => c.FechaInicio)
                .ToListAsync();
        }

        public async Task<List<Caso>> GetPorClienteAsync(int clienteId)
        {
            return await _db.Casos
                .Include(c => c.Actualizaciones)
                .Include(c => c.Archivos)
                .Where(c => c.ClienteId == clienteId)
                .OrderByDescending(c => c.FechaInicio)
                .ToListAsync();
        }

        public async Task<Caso?> GetByIdConDetallesAsync(int id)
        {
            return await _db.Casos
                .Include(c => c.Actualizaciones)
                    .ThenInclude(a => a.Archivos)
                .Include(c => c.Archivos)
                .Include(c => c.Pruebas)
                .Include(c => c.Comentarios)
                    .ThenInclude(com => com.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}