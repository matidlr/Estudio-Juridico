using EstudioJuridico.API2.Events;

namespace EstudioJuridico.API2.Observers.Interfaces
{
    public interface INotificacionObserver
    {
        Task OnFojaAgregada(FojaAgregadaEvent evento);
    }
}