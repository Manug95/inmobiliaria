using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Inmueble
{
    [Key]
    [Display(Name = "Nro Inmueble")]
    public int Id { get; set; }

    [Required]
    public int IdPropietario { get; set; }

    [Display(Name = "Dueño")]
    public Propietario? Duenio { get; set; }

    [Required]
    public int IdTipoInmueble { get; set; }

    [Display(Name = "Tipo Inmueble")]
    public TipoInmueble? Tipo { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "El maximo de caracteres es 50")]
    public string? Uso { get; set; }

    [Required]
    [Display(Name = "Cant. Ambientes")]
    public int CantidadAmbientes { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [DataType(DataType.Currency)]
    public decimal Precio { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "El maximo de caracteres es 100")]
    public string? Calle { get; set; }

    [Required]
    public uint NroCalle { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public bool Disponible { get; set; }

    public bool Borrado { get; set; }

    public Inmueble() { }

    public override string ToString()
    {
        return $"Dirección {Calle} {NroCalle}{(Duenio != null ? $" perteneciente a {Duenio?.ToString()}" : "")}";
    }

    public string Direccion()
    {
        return $"{Calle} {NroCalle}";
    }

    public string MostrarDatos()
    {
        Console.WriteLine(Tipo);
        return @$"
        Id: {Id}
        IdPropietario: {IdPropietario}
        IdTipoInmueble: {IdTipoInmueble}
        Uso: {Uso}
        CantidadAmbientes: {CantidadAmbientes}
        Precio: {Precio}
        Calle: {Calle}
        NroCalle: {NroCalle}
        Latitud: {Latitud}
        Longitud: {Longitud}";
    }
}
