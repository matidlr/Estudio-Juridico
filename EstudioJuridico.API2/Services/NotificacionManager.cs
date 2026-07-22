using EstudioJuridico.API2.Events;
using EstudioJuridico.API2.Observers.Interfaces;

namespace EstudioJuridico.API2.Services
{
    public class NotificacionManager
    {
        private readonly IEnumerable<INotificacionObserver> _observers;
        private readonly ILogger<NotificacionManager> _logger;

        public NotificacionManager(
            IEnumerable<INotificacionObserver> observers,
            ILogger<NotificacionManager> logger)
        {
            _observers = observers;
            _logger    = logger;
        }

        public async Task NotificarFojaAgregada(FojaAgregadaEvent evento)
        {
            foreach (var observer in _observers)
            {
                try
                {
                    await observer.OnFojaAgregada(evento);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en observer {Observer}", observer.GetType().Name);
                }
            }
        }
    }
}