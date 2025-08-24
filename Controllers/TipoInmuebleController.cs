using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;

namespace InmobiliariaGutierrezManuel.Controllers;

public class TipoInmuebleController : Controller
{
    private readonly TipoInmuebleRepository repo;

    public TipoInmuebleController()
    {
        repo = new TipoInmuebleRepository();
    }

    public IActionResult Index()
    {
        IList<TipoInmueble> tiposInmueble = repo.ListarTiposInmueble();
        TipoInmuebleViewModel tivm = new TipoInmuebleViewModel
        {
            TiposInmuebles = tiposInmueble,
            TipoInmueble = new TipoInmueble(),
            MensajeError = TempData["MensajeError"] as string
        };

        return View(tivm);
    }

    // public IActionResult Listar()
    // {
    //     IList<TipoInmueble> inquilinos = repo.ListarTiposInmueble();
    //     return View(inquilinos);
    // }

    [HttpPost]
    public IActionResult Guardar(TipoInmueble tipoInmueble)
    {
        if (ModelState.IsValid)
        {
            if (tipoInmueble.Id > 0)
            {
                repo.ActualizarTipoInmueble(tipoInmueble);
            }
            else
            {
                repo.InsertarTipoInmueble(tipoInmueble);
            }
            return RedirectToAction(nameof(Index));
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
        repo.EliminarTipoInmueble(id);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
