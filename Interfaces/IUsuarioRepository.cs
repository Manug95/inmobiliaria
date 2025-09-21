using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IUsuarioRepository
    {
        public IList<Usuario> ListarUsuarios(int? offset, int? limit);
        public Usuario? ObtenerUsuario(int id);

        public Usuario? ObtenerPorEmail(string email);
        public int InsertarUsuario(Usuario usuario);
        public bool ActualizarUsuario(Usuario usuario);
        public bool EliminarUsuario(int id);
        public int ContarUsuarios();
        public bool ActualizarContraseña(int id, string password);
    }
}