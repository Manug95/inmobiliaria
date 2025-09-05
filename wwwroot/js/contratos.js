import { getElementById, mostrarMensaje , mostrarPregunta} from "./frontUtils.js";

document.addEventListener("DOMContentLoaded", e => {
  const DETALLES = [];

  document.querySelectorAll("td .bi-trash").forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Contrato/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

  document.querySelectorAll("td .bi-file-earmark-text").forEach(i => {
    i.addEventListener("click", async e => {
      const idFila = e.target.id.split("-")[1];

      if (!DETALLES.includes(d => +d.id === +idFila)) {
        const respuesta = await fetch(`/Contrato/Buscar/${idFila}`);
        const contrato = await respuesta.json();
        DETALLES.push(contrato);
        agregarDatosAlModal(contrato);
      } else {
        agregarDatosAlModal(DETALLES.find(d => +d.id === +idFila));
      }

      const myModal = new bootstrap.Modal(getElementById('modal_detalle_contrato'), {});
      myModal.show();
    });
  });

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
});

function agregarDatosAlModal(datos) {
  getElementById("nro").textContent = datos.id;
  getElementById("propietario").textContent = `${datos.inmueble.duenio.apellido}, ${datos.inmueble.duenio.nombre}`;
  getElementById("inquilino").textContent = `${datos.inquilino.apellido}, ${datos.inquilino.nombre}`;
  getElementById("fIni").textContent = aFechaLocal(datos.fechaInicio.split("T")[0]);
  getElementById("fFin").textContent = aFechaLocal(datos.fechaFin.split("T")[0]);
  getElementById("monto").textContent = datos.montoMensual;
  getElementById("fTerm").textContent = aFechaLocal(datos.fechaTerminado?.split("T")[0]);
}

function aFechaLocal(fecha) {
  if (!fecha) return;
  return fecha.split("-").reverse().join("-");
}