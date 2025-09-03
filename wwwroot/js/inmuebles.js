import { getElementById, mostrarMensaje, mostrarPregunta } from "./frontUtils.js";


document.addEventListener("DOMContentLoaded", () => {

  mostrarMensaje(false, null);

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
  document.querySelectorAll(".bi-trash").forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Inmueble/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

  getElementById("filtro-disponible").addEventListener("change", e => {
    const select = e.target;
    // const btnConsultar = getElementById("consultar-btn");
    // btnConsultar.href = `/Inmueble?limit=10&offset=1&disp=${select.value}`;

    document.querySelectorAll(".page-link").forEach(a => {
      if (!a.classList.contains("disabled")) {
        const queryParams = a.href.split("?")[1].split("&");
        const offset = queryParams.filter(q => q.includes("offset"))[0];
        if (offset !== undefined) {
          const offsetValue = offset.split("=")[1];
          a.href = `/Inmueble?disp=${select.value}&limit=10&offset=${offsetValue}`;
        }
      }
    });

  });

  getElementById("consultar-btn").addEventListener("click", e => {
    const a = e.target;
    const propietario = getElementById("propietario").value;
    const disponibilidad = getElementById("filtro-disponible").value;

    a.href = `/Inmueble?limit=10&offset=1`;
    if (disponibilidad !== undefined && disponibilidad.trim().length > 0) a.href += `&disp=${disponibilidad}`;
    if (propietario !== undefined && propietario.trim().length > 0) a.href += `&prop=${propietario}`;

  });

});

// function eventoClicksDeLasPaginasDelPaginador(paginador) {
//   return async () => {
//     const propietario = getElementById("propietario").value;
//     const disponibilidad = getElementById("filtro-disponible").value;

//     const offset = (paginador.paginaActual - 1) * paginador.resultadosPorPagina;
//     const limit = paginador.resultadosPorPagina;
//     const otherParams = [];
//     if (disponibilidad !== undefined && disponibilidad.lenght > 0) otherParams.push(`disp=${disponibilidad}`);
//     if (propietario !== undefined && propietario.lenght > 0) otherParams.push(`prop=${propietario}`);
//     // const { order, orderType } = obtenerOpcionesDeConsulta(paginador.instanciaPaginador);

//     const datos = await paginador.enviarPeticion({ offset, limit, order, orderType, otherParams: otherParams.join("&") });
    
//     if (datos) {
//       const rutas = {
//         actualizar: "/Inmueble/FormularioInmueble"
//       };
//       renderizarTabla(datos, rutas); //refactorizar esta funcion
//     } else {
//       paginador.resetCantidadPaginadores();
//       mostrarMensaje(false, "No se encontraron datos");
//     }

//     paginador.actualizarPaginador();
//   }
// }
