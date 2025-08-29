using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;

namespace InmobiliariaGutierrezManuel.Controllers;

public class PropietarioController : Controller
{
    private readonly PropietarioRepository repo;

    public PropietarioController()
    {
        repo = new PropietarioRepository();
    }

    public IActionResult Index(string? nomApe, string? orderBy, string? order, int offset = 1, int limit = 10)
    {
        IList<Propietario> propietarios = repo.ListarPropietarios(nomApe, orderBy, order, offset, limit);
        int cantidadPropietarios = repo.ContarPropietarios();
        
        ViewBag.cantPag = Math.Ceiling( (decimal)cantidadPropietarios / limit );
        // ViewBag.offsetSiguiente = offset.HasValue ? offset.Value + 1 : 2;
        ViewBag.offsetSiguiente = offset + 1;
        // ViewBag.offsetAnterior = offset.HasValue ? offset.Value - 1 : 0;
        ViewBag.offsetAnterior = offset - 1;

        PropietarioViewModel pvm = new PropietarioViewModel
        {
            Propietarios = propietarios,
            Propietario = new Propietario(),
            MensajeError = TempData["MensajeError"] as string
        };

        return View(pvm);
    }

    public IActionResult Listar(string? nomApe, string? orderBy, string? order, int? offset = 1, int? limit = 10)
    {
        IList<Propietario> propietarios = repo.ListarPropietarios(nomApe, orderBy, order, offset, limit);
        return Json(new { datos = propietarios } );
    }

    // public IActionResult Buscar(int id = 0, string? dni = null)
    // {
    //     Propietario? propietario = repo.ObtenerPropietario(id, dni);
    //     return View(propietario);
    //     // return View();
    // }

    [HttpPost]
    public IActionResult Guardar(Propietario propietario)
    {
        // if (repo.ObtenerPropietario(null, propietario.Dni) != null)
        //     ModelState.AddModelError("Dni", "El DNI ya está registrado.");
        // if (repo.BuscarPorEmail(propietario.Email))
        //     ModelState.AddModelError("Email", "El E-Mail ya está registrado.");
        // if (repo.BuscarPorTelefono(propietario.Telefono))
        //     ModelState.AddModelError("Telefono", "El teléfono ya está registrado.");

        //ahora el tema es ¿como avisar al usuario?
        if (ModelState.IsValid)
        {
            if (propietario.Id > 0)
            {
                repo.ActualizarPropietario(propietario);
            }
            else
            {
                repo.InsertarPropietario(propietario);
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

    // public IActionResult PropietarioForm(int id = 0)
    // {
    //     if (id > 0)
    //     {
    //         Propietario? propietario = repo.ObtenerPropietario(id, null);
    //         return View(propietario);
    //     }
        
    //     return View();
    // }

    public IActionResult Eliminar(int id)
    {
        repo.EliminarPropietario(id);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
