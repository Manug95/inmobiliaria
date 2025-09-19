using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaGutierrezManuel.Models;
using InmobiliariaGutierrezManuel.Repositories;
using InmobiliariaGutierrezManuel.Models.ViewModels;
using System.Text.Json.Serialization;

namespace InmobiliariaGutierrezManuel.Controllers;

public class ContratoController : Controller
{
    private readonly ContratoRepository repo;
    private readonly InmuebleRepository repoInmueble;
    private readonly PagoRepository repoPago;
    private readonly InquilinoRepository repoInquilino;

    public ContratoController()
    {
        repo = new ContratoRepository();
        repoInmueble = new InmuebleRepository();
        repoPago = new PagoRepository();
        repoInquilino = new InquilinoRepository();
    }

    public IActionResult Index(int? idInm, int offset = 1, int limit = 10)
    {
        IList<Contrato> contratos = repo.ListarContratos(offset, limit, idInm);
        int cantidadContratos = repo.ContarContratos(idInm);

        ViewBag.cantPag = Math.Ceiling((decimal)cantidadContratos / limit);
        ViewBag.offsetSiguiente = offset + 1;
        ViewBag.offsetAnterior = offset - 1;
        ViewBag.idInm = idInm;
        ViewBag.contratos = contratos;

        ViewBag.mensajeError = "";

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
                DateTime desde = (DateTime)contrato.FechaInicio!;
                DateTime hasta = (DateTime)contrato.FechaFin!;
                if (repo.EstaDisponible(desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), (int)contrato.IdInmueble!))
                    repo.InsertarContrato(contrato);
                else
                {
                    TempData["MensajeError"] = "El Inmueble ya está en un contrato entre las fechas indicadas";
                    return RedirectToAction(nameof(FormularioContrato), new { desde = desde.ToString("yyyy-MM-dd"), hasta = hasta.ToString("yyyy-MM-dd"), idInq = contrato.IdInquilino, idInm = contrato.IdInmueble });
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
        if (idInq > 0) ViewBag.inquilino = repoInquilino.ObtenerInquilino(idInq, null);
        ViewBag.IdInquilino = idInq;
        ViewBag.id = id;
        ViewBag.desde = desde;
        ViewBag.hasta = hasta;
        ViewBag.mensajeError = TempData["MensajeError"];
        return View(contrato);
    }

    [HttpPost]
    public IActionResult Terminar([FromBody] TerminarContratoViewModel vm)
    {
        if (ModelState.IsValid)
        {
            DateTime fechaTerminado = vm.FechaTerminado.HasValue ? vm.FechaTerminado!.Value : DateTime.Today;
            if (repo.TerminarContrato(vm.Id, fechaTerminado.ToString("yyyy-MM-dd")))
            {
                return Json(CalcularMulta(vm.Id));
            }
            else
                return Json(new { error = "No se pudo terminar el contrato" });
        }
        else
        {
            Console.WriteLine(vm.Id);
            Console.WriteLine(vm.FechaTerminado);
            return Json(new { error = "Datos incorrectos" });
        }
    }

    public IActionResult DetalleMulta(int id)
    {
        return Json(CalcularMulta(id));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private MultaViewModel CalcularMulta(int idContrato)
    {
        Contrato? contrato = repo.ObtenerContrato(idContrato);
        decimal deuda = 0;

        // Paso 1: calcular la duración total
        TimeSpan duracionTotalContrato = contrato!.FechaFin!.Value - contrato!.FechaInicio!.Value;

        // Paso 2: calcular la fecha de mitad
        DateTime fechaMitad = contrato.FechaInicio.Value + TimeSpan.FromTicks(duracionTotalContrato.Ticks / 2);

        // Paso 3: comparar
        decimal multa = contrato.FechaTerminado!.Value < fechaMitad ? contrato!.MontoMensual!.Value * 2 : contrato!.MontoMensual!.Value;

        int cantPagos = repoPago.ContarPagos(contrato.Id);
        int cantMesesAlquilado = (int)Math.Floor((contrato.FechaTerminado.Value - contrato.FechaInicio.Value).TotalDays / 30);
        int cantMesesDelContrato = (int)Math.Floor((contrato.FechaFin.Value - contrato.FechaInicio.Value).TotalDays / 30);
        if (cantPagos < cantMesesAlquilado)
            deuda = contrato.MontoMensual.Value * (cantMesesAlquilado - cantPagos);

        return new MultaViewModel
        {
            Multa = multa,
            Deuda = deuda,
            CantMesesDelContrato = cantMesesDelContrato,
            CantMesesAlquilado = cantMesesAlquilado,
            CantMesesPagados = cantPagos,
            IdContrato = idContrato
        };
    }
}
