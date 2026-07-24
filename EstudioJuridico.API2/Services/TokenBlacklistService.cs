namespace EstudioJuridico.API2.Services
{
    public class TokenBlacklistService
    {
        private readonly HashSet<string> _tokensInvalidados = new();

        public void Invalidar(string token)
        {
            _tokensInvalidados.Add(token);
        }

        public bool EsInvalido(string token)
        {
            return _tokensInvalidados.Contains(token);
        }
    }
}