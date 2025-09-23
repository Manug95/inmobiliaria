import { getElementById, mostrarMensaje, mostrarPregunta, getFormInputValue } from "./frontUtils.js";
import { 
  resetValidationInputStyle, 
  resetValidationErrorMessage, 
  validarDescripcionTipoInmueble,
  setInvalidInputStyle,
  setValidInputStyle,
  setValidationErrorMessage
} from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {
  const DETALLES = [];

  mostrarMensaje(false, null);

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
  document.querySelectorAll(".bi-trash")?.forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Pago/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

  document.querySelectorAll("td .bi-file-earmark-text")?.forEach(i => {
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

  document.querySelectorAll("td .bi-pencil-square").forEach(i => {
    i.addEventListener("click", e => {
      const idFIla = e.target.id.split("-")[1];
      const fila = getElementById(idFIla);
      const datosFila = getRawValues(fila);

      getElementById("Detalle-edit").value = datosFila.detalle;
      getElementById("Id").value = datosFila.id;
      getElementById("Fecha").value = datosFila.fecha;
      getElementById("Importe").value = datosFila.importe.split(" ")[1];

      resetValidationStatus();
      
      const myModal = new bootstrap.Modal(getElementById('modal_formulario_editar_pago'), {});
      myModal.show();
    });
  });

  const form = getElementById("form-edit-pago");
  form?.addEventListener("submit", (e) => {
    e.preventDefault();
    if (validarFormulario({ "Detalle-edit" : getFormInputValue("Detalle-edit") })) {
      setTimeout((form) => { form.submit(); }, 500, form);
    }

    return;
  });

});

function agregarDatosAlModal(pago) {
  getElementById("nro").textContent = pago.id;
  getElementById("fPago_detalle").textContent = aFechaLocal(pago.fecha?.split("T")[0]);
  getElementById("importePago_detalle").textContent = pago.importe;
  getElementById("tipoPago_detalle").textContent = pago.tipo;
  getElementById("detPago_detalle").textContent = pago.detalle;
  getElementById("estadoPago_detalle").textContent = !pago.estado ? "ANULADO" : "";
  getElementById("propietario_detalle").textContent = `${pago.contrato.inmueble.duenio.apellido}, ${pago.contrato.inmueble.duenio.nombre}`;
  getElementById("inquilino_detalle").textContent = `${pago.contrato.inquilino.apellido}, ${pago.contrato.inquilino.nombre}`;
  getElementById("tipo_inmueble_detalle").textContent = pago.contrato.inmueble.tipo.tipo;
  getElementById("uso_detalle").textContent = pago.contrato.inmueble.uso;
  getElementById("cant_amb_detalle").textContent = pago.contrato.inmueble.cantidadAmbientes;
  getElementById("precio_detalle").textContent = pago.contrato.inmueble.precio;
  getElementById("dir_detalle").textContent = `${pago.contrato.inmueble.calle} ${pago.contrato.inmueble.nroCalle}`;
  getElementById("lat_detalle").textContent = pago.contrato.inmueble.latitud;
  getElementById("long_detalle").textContent = pago.contrato.inmueble.longitud;
  getElementById("habilitado_detalle").textContent = pago.contrato.inmueble.disponible ? "SI" : "NO";
  getElementById("fIni_detalle").textContent = aFechaLocal(pago.contrato.fechaInicio?.split("T")[0]);
  getElementById("fFin_detalle").textContent = aFechaLocal(pago.contrato.fechaFin?.split("T")[0]);
  getElementById("monto_detalle").textContent = pago.contrato.montoMensual;
  getElementById("fTerm_detalle").textContent = aFechaLocal(pago.contrato.fechaTerminado?.split("T")[0]);
  getElementById("detPago_cobrado").textContent = `Cod: ${pago.idUsuarioCobrador} - ${pago.usuarioCobrador.apellido}, ${pago.usuarioCobrador.nombre}`;
  getElementById("detPago_anulado").textContent = pago.idUsuarioAnulador ? `Cod: ${pago.idUsuarioAnulador} - ${pago.usuarioAnulador.Apellido}, ${pago.usuarioAnulador.nombre}` : "";
}

function aFechaLocal(fecha) {
  if (!fecha) return;
  return fecha.split("-").reverse().join("-");
}

/**
 * @param {HTMLTableRowElement} raw 
 */
function getRawValues(raw) {
  const datos = raw.children;
  return {
    fecha: datos.item(1).innerText.trim(),
    importe: datos.item(2).innerText.trim(),
    detalle: datos.item(4).innerText.trim(),
    id: raw.id
  };
}

/**
 * @param {*} values 
 * @returns {Boolean}
 */
function validarFormulario(values) {
  const mapaValidador = new Map([
    ["Detalle-edit", validarDescripcionTipoInmueble]
  ]);

  const esValido =  Object.keys(values)
  .map(v => {
    const result = mapaValidador.get(v)(values[v]);
    if (result !== undefined) { setInvalidInputStyle(v); setValidationErrorMessage(`tpe_${v}`, result.errorMessage); } 
    else { setValidInputStyle(v); resetValidationErrorMessage(`tpe_${v}`); }
    return result === undefined;
  })
  .every(v => v);

  return esValido;
}

function resetValidationStatus() {
  resetValidationInputStyle("Detalle-edit");
  resetValidationErrorMessage("Detalle-edit");
}
