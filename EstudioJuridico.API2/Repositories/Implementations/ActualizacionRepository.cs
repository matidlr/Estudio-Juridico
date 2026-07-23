using EstudioJuridico.API2.Base;
using EstudioJuridico.API2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstudioJuridico.API2.Repositories.Implementations
{
    public class ActualizacionRepository : BaseRepository<Actualizacion>, IActualizacionRepository
    {
        public ActualizacionRepository(AppDbContext db) : base(db) { }

        public async Task<List<Actualizacion>> GetPorCasoAsync(int casoId, int? seccionId, string? busqueda)
        {
            var query = _db.Actualizaciones.Where(a => a.CasoId == casoId);

            if (seccionId.HasValue)
                query = query.Where(a => a.SeccionExpedienteId == seccionId);

            if (!string.IsNullOrEmpty(busqueda))
                query = query.Where(a =>
                    a.NroFoja!.Contains(busqueda) ||
                    a.Contenido.Contains(busqueda));

            return await query.OrderBy(a => a.NroFoja).ToListAsync();
        }

        public async Task<int> CountPorCasoAsync(int casoId, int? seccionId, string? busqueda)
        {
            var query = _db.Actualizaciones.Where(a => a.CasoId == casoId);

            if (seccionId.HasValue)
                query = query.Where(a => a.SeccionExpedienteId == seccionId);

            if (!string.IsNullOrEmpty(busqueda))
                query = query.Where(a =>
                    a.NroFoja!.Contains(busqueda) ||
                    a.Contenido.Contains(busqueda));

            return await query.CountAsync();
        }

        public async Task<bool> ExisteFojaAsync(int casoId, string nroFoja, int? excluirId = null)
        {
            var query = _db.Actualizaciones
                .Where(a => a.CasoId == casoId && a.NroFoja == nroFoja);

            if (excluirId.HasValue)
                query = query.Where(a => a.Id != excluirId);

            return await query.AnyAsync();
        }

        public async Task<Actualizacion> CreateAsync(Actualizacion actualizacion)
        {
            _db.Actualizaciones.Add(actualizacion);
            await _db.SaveChangesAsync();
            return actualizacion;
        }

        public async Task UpdateAsync(Actualizacion actualizacion)
        {
            _db.Actualizaciones.Update(actualizacion);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var actualizacion = await GetByIdAsync(id);
            if (actualizacion != null)
            {
                _db.Actualizaciones.Remove(actualizacion);
                await _db.SaveChangesAsync();
            }
        }
    }
}