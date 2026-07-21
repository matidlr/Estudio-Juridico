namespace EstudioJuridico.API2.Services.Interfaces
{
    public interface ICasoService
    {
        Task<List<Caso>> GetCasosDeCliente(int clienteId);
        Task<Caso> CrearCaso(CasoDTO dto, int abogadoIdPorDefecto);
        Task AgregarActualizacion(ActualizacionDTO dto, int autorId);
        Task NotificarCliente(int casoId);
    }
}