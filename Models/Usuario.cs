using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class Usuario
{
    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }
    public string? Rol { get; set; }
    public string? Avatar { get; set; }
    public IFormFile? AvatarFile { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public bool Activo { get; set; }

    public override string ToString()
    {
        return $"{Apellido}, {Nombre}";
    }
}