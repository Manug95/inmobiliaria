using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Se Requiera ingresar un email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "No es un email válido")]
    [StringLength(100, ErrorMessage = "El máximo de caracteres es 100")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Se Requiera ingresar una contraseña")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public LoginViewModel(){}

    public LoginViewModel(string email, string password)
    {
        Email = email;
        Password = password;
    }
}