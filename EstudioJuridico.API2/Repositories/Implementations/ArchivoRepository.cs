using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstudioJuridico.API2.Repositories.Implementations
{
    public class ArchivoRepository : BaseRepository<Archivo>, IArchivoRepository
    {
        public ArchivoRepository(AppDbContext db) : base(db) { }

        public async Task<List<Archivo>> GetPorCasoAsync(int casoId)
        {
            return await _db.Archivos
                .Where(a => a.CasoId == casoId)
                .ToListAsync();
        }

        public async Task<List<Archivo>> GetPorSeccionAsync(int casoId, int seccionId)
        {
            return await _db.Archivos
                .Where(a => a.CasoId == casoId && a.SeccionExpedienteId == seccionId)
                .ToListAsync();
        }

        public async Task<Archivo> CreateAsync(Archivo archivo)
        {
            _db.Archivos.Add(archivo);
            await _db.SaveChangesAsync();
            return archivo;
        }

        public async Task DeleteAsync(int id)
        {
            var archivo = await GetByIdAsync(id);
            if (archivo != null)
            {
                _db.Archivos.Remove(archivo);
                await _db.SaveChangesAsync();
            }
        }
    }
}