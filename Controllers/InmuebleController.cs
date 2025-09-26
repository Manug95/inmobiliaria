using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Models.ViewModels;
using InmobiliariaGutierrezManuel.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaGutierrezManuel.Controllers;

public class InmuebleController : Controller
{
    private readonly InmuebleRepository repo;
    private readonly TipoInmuebleRepository repoTipoInmueble;
    private readonly PropietarioRepository repoPropietario;

    public InmuebleController()
    {
        repo = new InmuebleRepository();
        repoTipoInmueble = new TipoInmuebleRepository();
        repoPropietario = new PropietarioRepository();
    }

    [Authorize]
    public IActionResult Index(string? prop, int idProp = 0, int offset = 1, int limit = 10, int disp = (int)Disponiblilidad.TODOS)
    {
        IList<Inmueble>? inmuebles;
        int cantidadInmuebles;

        if (idProp == 0)
        {
            inmuebles = repo.ListarInmuebles(disp, offset, limit, prop);
            cantidadInmuebles = repo.ContarInmuebles(disp);
        }
        else
        {
            inmuebles = repo.ListarInmueblesPorPropietario(idProp, offset, limit);
            cantidadInmuebles = inmuebles.Count;
        }


        IList<TipoInmueble> tiposInmuebles = repoTipoInmueble.ListarTiposInmueble();

        ViewBag.cantPag = Math.Ceiling((decimal)cantidadInmuebles / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;
        ViewBag.disponible = disp;
        ViewBag.propietario = idProp == 0 ? prop : (inmuebles.First().Duenio?.Apellido + " " + inmuebles.First().Duenio?.Nombre);
        ViewBag.idProp = idProp;

        InmuebleViewModel ivm = new InmuebleViewModel
        {
            Inmuebles = inmuebles,
            Inmueble = new Inmueble(),
            TiposInmuebles = tiposInmuebles,
            MensajeError = TempData["MensajeError"] as string
        };

        return View(ivm);
    }

    /*
        [Bind(Prefix = "InmuebleFormData")] es porque
        el InmuebleViewModel que le paso a la vista tiene como atributo un InmuebleFormData
        y al generar el HTML, los atributos name de los inputs se crean con este formato "InmuebleFormData.nombredelcampo"
        entoces con este Bind le digo al framework que tenga en cuenta eso para poder mapear los campos del formulario correctamente
    */
    [HttpPost]
    [Authorize]
    public IActionResult Guardar([Bind(Prefix = "InmuebleFormData")] InmuebleFormData inmuebleForm)
    {
        if (inmuebleForm.IdTipoInmueble == 0)
        {
            int idTipoInmuebleNuevo = repoTipoInmueble.InsertarTipoInmueble(
                new TipoInmueble
                {
                    Tipo = inmuebleForm.NuevoTipo,
                    Descripcion = inmuebleForm.NuevoTipoDescripcion
                }
            );
            inmuebleForm.IdTipoInmueble = idTipoInmuebleNuevo;
        }

        if (ModelState.IsValid)
        {
            Inmueble inmueble = new Inmueble
            {
                Calle = inmuebleForm.Calle,
                CantidadAmbientes = (int)inmuebleForm.CantidadAmbientes!,
                IdPropietario = inmuebleForm.IdPropietario,
                IdTipoInmueble = inmuebleForm.IdTipoInmueble,
                Latitud = inmuebleForm.Latitud != null ? (decimal)inmuebleForm.Latitud : 0,
                Longitud = inmuebleForm.Longitud != null ? (decimal)inmuebleForm.Longitud : 0,
                NroCalle = (uint)inmuebleForm.NroCalle!,
                Precio = (decimal)inmuebleForm.Precio!,
                Uso = inmuebleForm.Uso,
            };

            if (inmuebleForm.Id > 0)
            {
                inmueble.Id = inmuebleForm.Id;
                inmueble.Disponible = inmuebleForm.Disponible;
                repo.ActualizarInmueble(inmueble);
            }
            else
            {
                repo.InsertarInmueble(inmueble);
            }
            return RedirectToAction(nameof(Index));
        }
        else
        {
            string errorMsg = "";
            foreach (var estado in ModelState)
            {
                var campo = estado.Key;
                foreach (var error in estado.Value.Errors)
                {
                    errorMsg += $" - {error.ErrorMessage}";
                }
            }
            TempData["MensajeError"] = errorMsg;

            return RedirectToAction(nameof(Index));
        }

    }

    [Authorize]
    public IActionResult FormularioInmueble(int id = 0, int idProp = 0)
    {
        IList<TipoInmueble> tiposInmuebles = repoTipoInmueble.ListarTiposInmueble();
        IList<Propietario> propietarios = [];

        if (idProp > 0)
        {
            Propietario? prop = repoPropietario.ObtenerPropietario(idProp, null);
            propietarios.Add(prop!);
        }

        InmuebleFormData? inmuebleFormData = null;

        if (id > 0)
        {
            Inmueble? inmueble = repo.ObtenerInmueble(id);
            if (inmueble != null)
            {
                inmuebleFormData = new InmuebleFormData
                {
                    Id = inmueble.Id,
                    Calle = inmueble.Calle,
                    CantidadAmbientes = inmueble.CantidadAmbientes,
                    IdPropietario = inmueble.IdPropietario,
                    IdTipoInmueble = inmueble.IdTipoInmueble,
                    Latitud = inmueble.Latitud,
                    Longitud = inmueble.Longitud,
                    NroCalle = inmueble.NroCalle,
                    Precio = inmueble.Precio,
                    Uso = inmueble.Uso,
                    Disponible = inmueble.Disponible,
                    Duenio = inmueble.Duenio
                };
            }

        }

        InmuebleViewModel ivm = new InmuebleViewModel
        {
            Inmueble = new Inmueble(),
            TiposInmuebles = tiposInmuebles,
            Propietarios = propietarios,
            InmuebleFormData = inmuebleFormData ?? new InmuebleFormData()
        };

        return View(ivm);
    }

    [Authorize(Policy = "ADMIN")]
    public IActionResult Eliminar(int id)
    {
        repo.EliminarInmueble(id);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult Alquilar(string? desde, string? hasta, string? Uso, int? IdTipoInmueble, int? CantidadAmbientes, decimal? Precio, int idInq = 0, int offset = 1, int limit = 10)
    {
        IList<Inmueble> inmuebles = [];
        if (desde != null && hasta != null)
        {
            inmuebles = repo.ListarInmueblesParaAlquilar(desde, hasta, Uso, IdTipoInmueble, CantidadAmbientes, Precio, offset, limit);
            if (inmuebles.Count == 0) ViewBag.mensaje = "No se encontraron resultados";
        }
        IList<TipoInmueble>? tipoInmuebles = repoTipoInmueble.ListarTiposInmueble();

        ViewBag.tiposInmuebles = tipoInmuebles;
        ViewBag.inmuebles = inmuebles;
        ViewBag.idInquilino = idInq;

        ViewBag.cantPag = (int)Math.Ceiling((decimal)inmuebles.Count / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;

        ViewBag.desde = desde;
        ViewBag.hasta = hasta;
        ViewBag.uso = Uso;
        ViewBag.tipo = IdTipoInmueble;
        ViewBag.precio = Precio;
        ViewBag.cantAmb = CantidadAmbientes;

        return View(new Inmueble());
    }

    [Authorize]
    public IActionResult Buscar(int id)
    {
        Inmueble? inmueble = repo.ObtenerInmueble(id);
        return Json(new { inmueble });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
