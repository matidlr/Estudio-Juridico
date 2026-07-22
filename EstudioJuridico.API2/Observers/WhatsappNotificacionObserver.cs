using EstudioJuridico.API2.Events;
using EstudioJuridico.API2.Observers.Interfaces;
using EstudioJuridico.API2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstudioJuridico.API2.Observers
{
    public class WhatsAppNotificacionObserver : INotificacionObserver
    {
        private readonly IWhatsAppService _whatsapp;
        private readonly AppDbContext _db;

        public WhatsAppNotificacionObserver(IWhatsAppService whatsapp, AppDbContext db)
        {
            _whatsapp = whatsapp;
            _db       = db;
        }

        public async Task OnFojaAgregada(FojaAgregadaEvent evento)
        {
            var caso = await _db.Casos
                .Include(c => c.Cliente)
                    .ThenInclude(cl => cl.Preferencias)
                .Include(c => c.Cliente)
                    .ThenInclude(cl => cl.Usuario)
                .FirstOrDefaultAsync(c => c.Id == evento.CasoId);

            if (caso?.Cliente?.Preferencias?.RecibirPorWhatsApp == true &&
                caso.Cliente.Preferencias.WhatsAppConfirmado)
            {
                var mensaje = evento.NroFoja != null
                    ? $"Se agregó la foja {evento.NroFoja} en tu expediente: {evento.Caratula}"
                    : $"Hay una nueva actualización en tu expediente: {evento.Caratula}";

                await _whatsapp.Enviar(
                    caso.Cliente.Telefono,
                    mensaje
                );
            }
        }
    }
}