using InmobiliariaGutierrezManuel.Models;

namespace InmobiliariaGutierrezManuel.Interfaces
{
    public interface IContratoRepository
    {
        public IList<Contrato> ListarContratos(int? offset = null, int? limit = null, int? idInm = null, string? desde = null, string? hasta = null, string? fechaAVencer = null);
        public Contrato? ObtenerContrato(int id);
        public int InsertarContrato(Contrato contrato);
        public bool ActualizarContrato(Contrato contrato);
        public bool EliminarContrato(int id);
        public int ContarContratos(int? idInm = null, string? desde = null, string? hasta = null, string? fechaAVencer = null);
        public bool TerminarContrato(int id, string fecha, int idUsuario);
    }
}