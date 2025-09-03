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

    public IActionResult Index(string? order, int offset = 1, int limit = 10)
    {
        IList<Contrato> contratos = repo.ListarContratos(offset, limit);
        int cantidadContratos = repo.ContarContratos();
        
        ViewBag.cantPag = Math.Ceiling( (decimal)cantidadContratos / limit );
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;

        ViewBag.contratos = contratos;

        return View();
    }

    public IActionResult Listar(string? nomApe, string? orderBy, string? order, int? offset = 1, int? limit = 10)
    {
        IList<Contrato> contratos = repo.ListarContratos(offset, limit);
        return Json(new { datos = contratos } );
    }

    // public IActionResult Buscar(int id = 0, string? dni = null)
    // {
    //     Propietario? propietario = repo.ObtenerPropietario(id, dni);
    //     return View(propietario);
    //     // return View();
    // }

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

    public IActionResult FormularioContrato(int id = 0, int idInq = 0, int idInm = 0)
    {
        if (idInm > 0)
        {
            ViewBag.inmueble = repoInmueble.ObtenerInmueble(idInm);
        }
        ViewBag.IdInquilino = idInq;
        ViewBag.IdInmueble = idInm;
        return View(new Contrato());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
