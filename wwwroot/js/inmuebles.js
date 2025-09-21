import { getElementById, mostrarMensaje, mostrarPregunta } from "./frontUtils.js";


document.addEventListener("DOMContentLoaded", () => {
  const DETALLES = [];

  mostrarMensaje(false, null);

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
  document.querySelectorAll(".bi-trash")?.forEach(i => {
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

  document.querySelectorAll("td .bi-file-earmark-text").forEach(i => {
    i.addEventListener("click", async e => {
      const idFila = e.target.id.split("-")[1];

      if (DETALLES.findIndex(d => d.id === +idFila) < 0) {
        const respuesta = await fetch(`/Inmueble/Buscar/${idFila}`);
        const datos = await respuesta.json();
        DETALLES.push(datos.inmueble);
        agregarDatosAlModal(datos.inmueble);
      } else {
        agregarDatosAlModal(DETALLES.find(d => +d.id === +idFila));
      }

      const myModal = new bootstrap.Modal(getElementById('modal_detalle_inmueble'), {});
      myModal.show();
    });
  });

});

function agregarDatosAlModal(datos) {
  getElementById("nro").textContent = datos.id;
  getElementById("propietario_detalle").textContent = `${datos.duenio.apellido}, ${datos.duenio.nombre}`;
  getElementById("tipo_inmueble_detalle").textContent = datos.tipo.tipo;
  getElementById("uso_detalle").textContent = datos.uso;
  getElementById("cant_amb_detalle").textContent = datos.cantidadAmbientes;
  getElementById("precio_detalle").textContent = datos.precio;
  getElementById("dir_detalle").textContent = `${datos.calle} ${datos.nroCalle}`;
  getElementById("lat_detalle").textContent = datos.latitud;
  getElementById("long_detalle").textContent = datos.longitud;
  getElementById("habilitado_detalle").textContent = datos.disponible ? "SI" : "NO";
}
