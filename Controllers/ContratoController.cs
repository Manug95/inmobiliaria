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

        ViewBag.cantPag = Math.Ceiling((decimal)cantidadContratos / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;

        ViewBag.contratos = contratos;

        ViewBag.mensajeError = TempData["MensajeError"];

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
                // Console.WriteLine(contrato.IdInmueble);
                // Console.WriteLine(contrato.IdInquilino);
                // Console.WriteLine(contrato.MontoMensual);
                // Console.WriteLine(contrato.FechaInicio);
                // Console.WriteLine(contrato.FechaFin);
                // Console.WriteLine(contrato.FechaTerminado);
                // Console.WriteLine(contrato.Id);
                repo.ActualizarContrato(contrato);
            }
            else
            {
                DateTime desde = (DateTime)contrato.FechaInicio!;
                DateTime hasta = (DateTime)contrato.FechaFin!;
                if (repo.EstaDisponible(desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), (int)contrato.IdInmueble!))
                    repo.InsertarContrato(contrato);
                else
                {
                    TempData["MensajeError"] = "El Inmueble ya está en un contrato entre las fechas indicadas";
                    return RedirectToAction(nameof(FormularioContrato), new { desde=desde.ToString("yyyy-MM-dd"), hasta=hasta.ToString("yyyy-MM-dd"), idInq= contrato.IdInquilino, idInm=contrato.IdInmueble });
                }
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
                    errorMsg += $"{error.ErrorMessage}";
                }
            }
            TempData["MensajeError"] = errorMsg;

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
            ViewBag.IdInmueble = contrato?.IdInmueble;
        }
        else
        {
            ViewBag.IdInmueble = idInm;
        }
        ViewBag.IdInquilino = idInq;
        ViewBag.id = id;
        ViewBag.desde = desde;
        ViewBag.hasta = hasta;
        ViewBag.mensajeError = TempData["MensajeError"];
        return View(contrato);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
