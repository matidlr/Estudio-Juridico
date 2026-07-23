namespace EstudioJuridico.API2.Repositories.Interfaces
{
    public interface IArchivoRepository
    {
        Task<Archivo?> GetByIdAsync(int id);
        Task<List<Archivo>> GetPorCasoAsync(int casoId);
        Task<List<Archivo>> GetPorSeccionAsync(int casoId, int seccionId);
        Task<Archivo> CreateAsync(Archivo archivo);
        Task DeleteAsync(int id);
    }
}