namespace EstudioJuridico.API2.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetTodosAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente?> GetByUsuarioIdAsync(int usuarioId);
        Task<Cliente?> GetByIdConCasosAsync(int id);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
    }
}