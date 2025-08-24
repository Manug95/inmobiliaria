using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface ITipoInmuebleRepository
    {
        public IList<TipoInmueble> ListarTiposInmueble();
        public TipoInmueble? ObtenerTipoInmueble(int id);
        public int InsertarTipoInmueble(TipoInmueble tipoInmueble);
        public bool ActualizarTipoInmueble(TipoInmueble tipoInmueble);
        public bool EliminarTipoInmueble(int id);
    }
}