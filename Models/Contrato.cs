using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Contrato
{
    [Key]
    [Display(Name = "N°")]
    public int Id { get; set; }
    public int? IdInquilino { get; set; }
    public Inquilino? Inquilino { get; set; }
    public int? IdInmueble { get; set; }
    public Inmueble? Inmueble { get; set; }
    public int IdUsuarioContratador { get; set; }
    public Usuario? UsuarioContratador { get; set; }
    public int IdUsuarioTerminador { get; set; }
    public Usuario? UsuarioTerminador { get; set; }
    [DataType(DataType.Currency)]
    public decimal? MontoMensual { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? FechaInicio { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? FechaFin { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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
