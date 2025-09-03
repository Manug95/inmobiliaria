using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Contrato
{
    [Key]
    public int Id { get; set; }
    public int? IdInquilino { get; set; }
    public Inquilino? Inquilino { get; set; }
    public int? IdInmueble { get; set; }
    public Inmueble? Inmueble { get; set; }
    public int IdUsuarioContratador { get; set; }
    public int IdUsuarioTerminador { get; set; }
    public decimal? MontoMensual { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public DateTime? FechaTerminado { get; set; }
    public bool Borrado { get; set; }

    public Contrato() { }

    public override string ToString()
    {
        return @$"
        IdInquilino: {IdInquilino}
        IdInmueble: {IdInmueble}
        MontoMensual: {MontoMensual}
        FechaInicio: {FechaInicio}
        FechaFin: {FechaFin}
        ";
    }
}
