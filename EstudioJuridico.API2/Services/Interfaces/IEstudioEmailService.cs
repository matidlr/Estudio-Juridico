namespace EstudioJuridico.API2.Services.Interfaces
{
    public interface IEstudioEmailService
    {
        Task Enviar(string destinatario, string asunto, string mensaje);
    }
}