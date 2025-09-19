using System.ComponentModel.DataAnnotations;

namespace InmobiliariaGutierrezManuel.Models.ViewModels;

public class TerminarContratoViewModel
{
    [Required]
    public int Id { get; set; }

    public DateTime? FechaTerminado { get; set; }
}
