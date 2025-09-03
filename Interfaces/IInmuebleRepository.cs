using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IInmuebleRepository
    {
        public IList<Inmueble> ListarInmuebles(int disponible, int? offset = null, int? limit = null, string? nomApeProp = null);
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
        public int ContarInmuebles(int? disponible);
        public IList<Inmueble> ListarInmueblesParaAlquilar(string desde, string hasta, string uso, int tipo, int cantAmb, decimal precio, int offset, int limit);
    }
}