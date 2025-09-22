using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IPagoRepository
    {
        public IList<Pago> ListarPagos(int? offset = null,  int? limit = null, int? idCon = null);
        public Pago? ObtenerPago(int id);
        public int InsertarPago(Pago pago);
        public bool ActualizarPago(Pago pago);
        public bool EliminarPago(int id, int idUsuario);
        public int ContarPagos(int? idCon = null);
    }
}