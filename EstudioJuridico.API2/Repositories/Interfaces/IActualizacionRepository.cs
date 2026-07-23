namespace EstudioJuridico.API2.Repositories.Interfaces
{
    public interface IActualizacionRepository
    {
        Task<Actualizacion?> GetByIdAsync(int id);
        Task<List<Actualizacion>> GetPorCasoAsync(int casoId, int? seccionId, string? busqueda);
        Task<int> CountPorCasoAsync(int casoId, int? seccionId, string? busqueda);
        Task<Actualizacion> CreateAsync(Actualizacion actualizacion);
        Task UpdateAsync(Actualizacion actualizacion);
        Task DeleteAsync(int id);
        Task<bool> ExisteFojaAsync(int casoId, string nroFoja, int? excluirId = null);
    }
}