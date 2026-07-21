namespace EstudioJuridico.API2.Services.Interfaces
{
    public interface IWhatsAppService
    {
        Task Enviar(string numeroDestino, string mensaje);
    }
}