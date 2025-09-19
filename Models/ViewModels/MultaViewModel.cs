namespace InmobiliariaGutierrezManuel.Models.ViewModels;

public class MultaViewModel
{
    public decimal Multa { get; set; }
    public decimal Deuda { get; set; }
    public int CantMesesPagados { get; set; }
    public int CantMesesAlquilado { get; set; }
    public int CantMesesDelContrato { get; set; }
    public int IdContrato { get; set; }
}