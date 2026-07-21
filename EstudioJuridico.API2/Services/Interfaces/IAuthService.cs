namespace EstudioJuridico.API2.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario?> Registrar(RegisterDTO dto);
        Task<string?> Login(LoginDTO dto);
        Task<Usuario?> RegistrarAdmin(RegisterDTO dto);
        Task<Usuario?> RegistrarAbogado(RegisterDTO dto);
        string GenerarToken(Usuario usuario);
    }
}