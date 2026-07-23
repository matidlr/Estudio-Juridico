using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstudioJuridico.API2.Repositories.Implementations
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext db) : base(db) { }

        public async Task<List<Cliente>> GetTodosAsync()
        {
            return await _db.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Preferencias)
                .Include(c => c.Casos)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _db.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Preferencias)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<Cliente?> GetByIdConCasosAsync(int id)
        {
            return await _db.Clientes
                .Include(c => c.Usuario)
                .Include(c => c.Preferencias)
                .Include(c => c.Casos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _db.Clientes.Update(cliente);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await GetByIdAsync(id);
            if (cliente != null)
            {
                _db.Clientes.Remove(cliente);
                await _db.SaveChangesAsync();
            }
        }
    }
}