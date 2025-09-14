import { getElementById, mostrarMensaje, mostrarPregunta } from "./frontUtils.js";


document.addEventListener("DOMContentLoaded", () => {
  const DETALLES = [];

  mostrarMensaje(false, null);

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
  document.querySelectorAll(".bi-trash").forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Pago/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

  document.querySelectorAll("td .bi-file-earmark-text").forEach(i => {
    i.addEventListener("click", async e => {
      const idFila = e.target.id.split("-")[1];

      if (DETALLES.findIndex(d => d.id === +idFila) < 0) {
        const respuesta = await fetch(`/Pago/Buscar/${idFila}`);
        const datos = await respuesta.json();
        DETALLES.push(datos.pago);
        agregarDatosAlModal(datos.pago);
      } else {
        agregarDatosAlModal(DETALLES.find(d => d.id === +idFila));
      }

      const myModal = new bootstrap.Modal(getElementById('modal_detalle_pago'), {});
      myModal.show();
    });
  });

});

function agregarDatosAlModal(datos) {
  getElementById("nro").textContent = datos.id;
  getElementById("fPago_detalle").textContent = aFechaLocal(datos.fecha?.split("T")[0]);
  getElementById("importePago_detalle").textContent = datos.importe;
  getElementById("detPago_detalle").textContent = datos.detalle;
  getElementById("propietario_detalle").textContent = `${datos.contrato.inmueble.duenio.apellido}, ${datos.contrato.inmueble.duenio.nombre}`;
  getElementById("inquilino_detalle").textContent = `${datos.contrato.inquilino.apellido}, ${datos.contrato.inquilino.nombre}`;
  getElementById("tipo_inmueble_detalle").textContent = datos.contrato.inmueble.tipo.tipo;
  getElementById("uso_detalle").textContent = datos.contrato.inmueble.uso;
  getElementById("cant_amb_detalle").textContent = datos.contrato.inmueble.cantidadAmbientes;
  getElementById("precio_detalle").textContent = datos.contrato.inmueble.precio;
  getElementById("dir_detalle").textContent = `${datos.contrato.inmueble.calle} ${datos.contrato.inmueble.nroCalle}`;
  getElementById("lat_detalle").textContent = datos.contrato.inmueble.latitud;
  getElementById("long_detalle").textContent = datos.contrato.inmueble.longitud;
  getElementById("habilitado_detalle").textContent = datos.contrato.inmueble.disponible ? "SI" : "NO";
  getElementById("fIni_detalle").textContent = aFechaLocal(datos.contrato.fechaInicio?.split("T")[0]);
  getElementById("fFin_detalle").textContent = aFechaLocal(datos.contrato.fechaFin?.split("T")[0]);
  getElementById("monto_detalle").textContent = datos.contrato.montoMensual;
  getElementById("fTerm_detalle").textContent = aFechaLocal(datos.contrato.fechaTerminado?.split("T")[0]);
}

function aFechaLocal(fecha) {
  if (!fecha) return;
  return fecha.split("-").reverse().join("-");
}
