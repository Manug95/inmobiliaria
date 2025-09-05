using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;

namespace InmobiliariaGutierrezManuel.Controllers;

public class ContratoController : Controller
{
    private readonly ContratoRepository repo;
    private readonly InmuebleRepository repoInmueble;

    public ContratoController()
    {
        repo = new ContratoRepository();
        repoInmueble = new InmuebleRepository();
    }

    public IActionResult Index(int offset = 1, int limit = 10)
    {
        IList<Contrato> contratos = repo.ListarContratos(offset, limit);
        int cantidadContratos = repo.ContarContratos();
        
        ViewBag.cantPag = Math.Ceiling( (decimal)cantidadContratos / limit );
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;

        ViewBag.contratos = contratos;

        return View(new Contrato());
    }

    // public IActionResult Listar(string? nomApe, int? offset = 1, int? limit = 10)
    // {
    //     IList<Contrato> contratos = repo.ListarContratos(offset, limit);
    //     return Json(new { datos = contratos } );
    // }

    public IActionResult Buscar(int id)
    {
        Contrato? contrato = repo.ObtenerContrato(id);
        return Json(contrato);
    }

    [HttpPost]
    public IActionResult Guardar(Contrato contrato)
    {
        if (ModelState.IsValid)
        {
            if (contrato.Id > 0)
            {
                repo.ActualizarContrato(contrato);
            }
            else
            {
                repo.InsertarContrato(contrato);
            }
            return RedirectToAction(nameof(FormularioContrato));
        }
        else
        {
            string errorMsg = "<ul>";
            foreach (var estado in ModelState)
            {
                var campo = estado.Key;
                foreach (var error in estado.Value.Errors)
                {
                    errorMsg += $"<li class=\"text-danger fs-5\"><strong>{error.ErrorMessage}</strong></li>";
                }
            }
            TempData["MensajeError"] = errorMsg + "</ul>";

            return RedirectToAction(nameof(Index));
        }
        
    }

    public IActionResult Eliminar(int id)
    {
        repo.EliminarContrato(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult FormularioContrato(string? desde, string? hasta, int id = 0, int idInq = 0, int idInm = 0)
    {
        Contrato? contrato = new Contrato();
        if (desde != null && hasta != null)
        {
            var desdeArr = desde.Split("-");
            var hastaArr = hasta.Split("-");

            contrato.FechaInicio = new DateTime(int.Parse(desdeArr[0]), int.Parse(desdeArr[1]), int.Parse(desdeArr[2]));
            contrato.FechaFin = new DateTime(int.Parse(hastaArr[0]), int.Parse(hastaArr[1]), int.Parse(hastaArr[2]));
        }
        if (idInm > 0)
        {
            ViewBag.inmueble = repoInmueble.ObtenerInmueble(idInm);
        }
        if (id > 0)
        {
            contrato = repo.ObtenerContrato(id);//ponerle los datos del contrato para la modificacion
        }
        ViewBag.IdInquilino = idInq;
        ViewBag.IdInmueble = idInm;
        ViewBag.desde = desde;
        ViewBag.hasta = hasta;
        return View(contrato);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
