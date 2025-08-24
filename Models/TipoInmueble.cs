using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class TipoInmueble
{
    [Key]
    public int Id { get; set; }

    [StringLength(25, ErrorMessage = "El maximo de caracteres es 25")]
    [Required(ErrorMessage="El nombre de tipo es requerido")]
    public string? Tipo { get; set; }

    [StringLength(255, ErrorMessage = "El maximo de caracteres es 255")]
    public string? Descripcion { get; set; }

    public TipoInmueble() { }

    public override string ToString()
    {
        return $"{Tipo}";
    }

}