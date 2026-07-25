using Ganss.Xss;

namespace EstudioJuridico.API2.Services
{
    public class SanitizadorService
    {
        private readonly HtmlSanitizer _sanitizer;

        public SanitizadorService()
        {
            _sanitizer = new HtmlSanitizer();
            // No permitimos ningún HTML, solo texto plano
            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedAttributes.Clear();
        }

        public string Limpiar(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return _sanitizer.Sanitize(input).Trim();
        }
    }
}