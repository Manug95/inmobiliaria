using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaGutierrezManuel.Controllers;

public class InquilinoController : Controller
{
    private readonly InquilinoRepository repo;

    public InquilinoController()
    {
        repo = new InquilinoRepository();
    }

    [Authorize]
    public IActionResult Index(string? nomApe, string? orderBy, string? order, int offset = 1, int limit = 10)
    {
        IList<Inquilino> inquilinos = repo.ListarInquilinos(nomApe, orderBy, order, offset, limit);
        int cantidadInquilinos = repo.ContarInquilinos();

        ViewBag.cantPag = Math.Ceiling((decimal)cantidadInquilinos / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;

        InquilinoViewModel ivm = new InquilinoViewModel
        {
            Inquilinos = inquilinos,
            Inquilino = new Inquilino(),
            MensajeError = TempData["MensajeError"] as string
        };

        return View(ivm);
    }

    public IActionResult Listar(string? nomApe, string? orderBy, string? order, int? offset = 1, int? limit = 10)
    {
        IList<Inquilino> inquilinos = repo.ListarInquilinos(nomApe, orderBy, order, offset, limit);
        return Json(new { datos = inquilinos } );
    }

    // public IActionResult Buscar(int id = 0, string? dni = null)
    // {
    //     Inquilino? inquilino = repo.ObtenerInquilino(id, dni);
    //     return View(inquilino);
    // }

    [HttpPost]
    [Authorize]
    public IActionResult Guardar(Inquilino inquilino)
    {
        // if (repo.ObtenerInquilino(null, inquilino.Dni) != null)
        //     ModelState.AddModelError("Dni", "El DNI ya está registrado.");
        // if (repo.BuscarPorEmail(inquilino.Email))
        //     ModelState.AddModelError("Email", "El E-Mail ya está registrado.");
        // if (repo.BuscarPorTelefono(inquilino.Telefono))
        //     ModelState.AddModelError("Telefono", "El teléfono ya está registrado.");

        if (ModelState.IsValid)
        {
            if (inquilino.Id > 0)
            {
                repo.ActualizarInquilino(inquilino);
            }
            else
            {
                repo.InsertarInquilino(inquilino);
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

    [Authorize(Policy = "ADMIN")]
    public IActionResult Eliminar(int id)
    {
        repo.EliminarInquilino(id);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
