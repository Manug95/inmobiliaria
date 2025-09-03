import Paginador from "./paginador.js";
import { getElementById, mostrarMensaje } from "./frontUtils.js";
import { renderizarTabla } from "./tablas.js";

document.addEventListener("DOMContentLoaded", e => {
    const paginador = new Paginador("/Contratos/Listar");
    paginador.setFuncionEnviarPeticionPaginador(eventoClicksDeLasPaginasDelPaginador(paginador.instanciaPaginador));

    //aca se einicia todo cuado se quiere hacer la busqueda
    //despues veo que uso para la busqueda y que evento uso
    getElementById("...").addEventListener("...", e => {});
});

function eventoClicksDeLasPaginasDelPaginador(paginador) {
  return async () => {
    const idDepositoSeleccionado = getElementById("deposito-nac").value;

    const offset = (paginador.paginaActual - 1) * paginador.resultadosPorPagina;
    const limit = paginador.resultadosPorPagina;
    const { order, orderType } = obtenerOpcionesDeConsulta(paginador.instanciaPaginador);

    const datos = await paginador.enviarPeticion(idDepositoSeleccionado, { offset, limit, order, orderType, otherParams });
    
    //despues veo con que formato traigo el objeto con los datos
    if (datos) {
      renderizarTabla(datos, rutaDescarte); //refactorizar esta funcion
    } else {
      paginador.resetCantidadPaginadores();
      mostrarMensaje(false, "No se encontraron datos");
    }

    paginador.actualizarPaginador();
  }
}

function obtenerOpcionesDeConsulta(paginador) {
  const cantResults = getElementById("cantidad-resultados").value;
  const orden = getElementById("orden").value;
  const tipoOrden = getElementById("tipo-orden").value;

  const order = orden !== "" ? orden : null;
  const orderType = tipoOrden !== "" ? tipoOrden : null;

  if (cantResults !== "") paginador.resultadosPorPagina = +cantResults;

  return { order, orderType };
}