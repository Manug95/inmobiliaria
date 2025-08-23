using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IInquilinoRepository
    {
        public IList<Inquilino> ListarInquilinos(
            string? nomApe = null,
            string? orderBy = null,
            string? order = "ASC",
            int? offset = null,
            int? limit = null
        );
        public Inquilino? ObtenerInquilino(int? id, string? dni);
        public int InsertarInquilino(Inquilino inquilino);
        public bool ActualizarInquilino(Inquilino inquilino);
        public bool EliminarInquilino(int id);
        public int ContarInquilinos();
    }
}