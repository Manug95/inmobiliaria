using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IPropietarioRepository
    {
        public IList<Propietario> ListarPropietarios(
            string? nomApe = null,
            string? orderBy = null,
            string? order = "ASC",
            int? offset = null,
            int? limit = null
        );
        public Propietario? ObtenerPropietario(int? id, string? dni);
        public int InsertarPropietario(Propietario propietario);
        public bool ActualizarPropietario(Propietario propietario);
        public bool EliminarPropietario(int id);
        public int ContarPropietarios();
    }
}