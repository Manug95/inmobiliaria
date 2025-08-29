using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IInmuebleRepository
    {
        public IList<Inmueble> ListarInmuebles(int? offset = null, int? limit = null);
        public IList<Inmueble> ListarInmueblesPorPropietario(
            string? nomApePropietario = null,
            string? orderBy = null,
            string? order = "ASC",
            int? offset = null,
            int? limit = null
        );
        public Inmueble? ObtenerInmueble(int id);
        public int InsertarInmueble(Inmueble inmueble);
        public bool ActualizarInmueble(Inmueble inmueble);
        public bool EliminarInmueble(int id);
        public int ContarInmuebles();
    }
}