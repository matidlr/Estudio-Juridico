using EstudioJuridico.API2.Events;
using EstudioJuridico.API2.Observers.Interfaces;
using EstudioJuridico.API2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EstudioJuridico.API2.Observers
{
    public class EmailNotificacionObserver : INotificacionObserver
    {
        private readonly IEstudioEmailService _email;
        private readonly AppDbContext _db;

        public EmailNotificacionObserver(IEstudioEmailService email, AppDbContext db)
        {
            _email = email;
            _db    = db;
        }

        public async Task OnFojaAgregada(FojaAgregadaEvent evento)
        {
            var caso = await _db.Casos
                .Include(c => c.Cliente)
                    .ThenInclude(cl => cl.Preferencias)
                .Include(c => c.Cliente)
                    .ThenInclude(cl => cl.Usuario)
                .FirstOrDefaultAsync(c => c.Id == evento.CasoId);

            if (caso?.Cliente?.Preferencias?.RecibirPorEmail == true &&
                caso.Cliente.Preferencias.EmailConfirmado)
            {
                var mensaje = evento.NroFoja != null
                    ? $"Se agregó la foja {evento.NroFoja} en tu expediente: {evento.Caratula}"
                    : $"Hay una nueva actualización en tu expediente: {evento.Caratula}";

                await _email.Enviar(
                    caso.Cliente.Usuario.Email,
                    "Nueva actualización en tu expediente",
                    mensaje
                );
            }
        }
    }
}