using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaGutierrezManuel.Controllers;

public class PagoController : Controller
{
    private readonly PagoRepository repo;
    private readonly ContratoRepository repoContrato;

    public PagoController()
    {
        repo = new PagoRepository();
        repoContrato = new ContratoRepository();
    }

    [Authorize]
    public IActionResult Index(int? idCon, int offset = 1, int limit = 10)
    {
        IList<Pago> pagos = repo.ListarPagos(offset, limit, idCon);
        int cantidadPagos = repo.ContarPagos();

        ViewBag.cantPag = Math.Ceiling((decimal)cantidadPagos / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;

        ViewBag.pagos = pagos;

        ViewBag.mensajeError = TempData["MensajeError"];

        return View(new Pago());
    }

    public IActionResult Listar(int offset = 1, int limit = 10)
    {
        IList<Pago> pagos = repo.ListarPagos(offset, limit);
        return Json(new { datos = pagos } );
    }

    public IActionResult Buscar(int id)
    {
        Pago? pago = repo.ObtenerPago(id);
        return Json(new { pago });
    }

    [HttpPost]
    [Authorize]
    public IActionResult Guardar(Pago pago)
    {
        if (ModelState.IsValid)
        {
            if (pago.Id > 0)
            {
                Pago? pagoAactualizar = repo.ObtenerPago(pago.Id);
                if (pagoAactualizar != null) pagoAactualizar.Detalle = pago.Detalle;
                repo.ActualizarPago(pagoAactualizar ?? pago);
            }
            else
            {
                // if (pago.Importe > repo.ObtenerSumaMultaAlquiler(pago.IdContrato))
                // {
                //     TempData["MensajeError"] = "El importe ingresado es mayor a la multa";
                //     return RedirectToAction(nameof(FormularioPago), new {idCon=pago.IdContrato, multa=pago.Importe});
                // }
                string idUsuario = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value!;
                pago.IdUsuarioCobrador = int.Parse(idUsuario);
                repo.InsertarPago(pago);
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
        repo.EliminarPago(id, int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value!));
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult FormularioPago(decimal? multa, int id = 0, int idCon = 0)
    {
        // Pago? pago;

        // if (id > 0) pago = repo.ObtenerPago(id);
        // else pago = new Pago{ IdContrato = idCon };

        if (idCon > 0)
        {
            ViewBag.contrato = repoContrato.ObtenerContrato(idCon);
        }

        if (multa.HasValue)
        {
            ViewBag.multa = multa.Value;
        }

        ViewBag.mensajeError = TempData["MensajeError"];

        return View(new Pago());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
