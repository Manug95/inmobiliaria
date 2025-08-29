using InmobiliariaGutierrezManuel.Models.ViewModels;

namespace InmobiliariaGutierrezManuel.Models.ViewModels;

public class InmuebleViewModel
{
    public IList<Inmueble>? Inmuebles { get; set; }
    public Inmueble? Inmueble { get; set; }
    public InmuebleFormData? InmuebleFormData { get; set; }
    public IEnumerable<TipoInmueble>? TiposInmuebles { get; set; }
    public IEnumerable<Propietario>? Propietarios { get; set; }
    public string? MensajeError { get; set; }
}
