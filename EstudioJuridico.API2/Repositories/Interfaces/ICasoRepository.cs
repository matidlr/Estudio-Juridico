namespace EstudioJuridico.API2.Repositories.Interfaces
{
    public interface ICasoRepository
    {
        Task<List<Caso>> GetTodosAsync();
        Task<List<Caso>> GetPorClienteAsync(int clienteId);
        Task<Caso?> GetByIdAsync(int id);
        Task<Caso?> GetByIdConDetallesAsync(int id);
        Task<Caso> CreateAsync(Caso caso);
        Task UpdateAsync(Caso caso);
        Task DeleteAsync(int id);
    }
}