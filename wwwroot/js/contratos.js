import { agregarClases, createElement, getElementById, mostrarMensaje , mostrarPregunta, removerClases} from "./frontUtils.js";
import { setInvalidInputStyle, setValidationErrorMessage, setValidInputStyle, resetValidationErrorMessage } from "./validaciones.js";

document.addEventListener("DOMContentLoaded", e => {
  let modalTerminarContrato;

  mostrarMensaje(false, null);

  const formTerminarContrato = getElementById("form-terminar_contrato");
  formTerminarContrato?.addEventListener("submit", async e => {
    e.preventDefault();

    const fecha = getElementById("fecha_term_form").value;
    // const [anio, _mes, _dia] = fecha.split("-");
    if (fecha.length > 0 && fecha.split("-")[0]?.length != 4) {
      setInvalidInputStyle("fecha_term_form");
      setValidationErrorMessage("tpe_fecha_term_form", "No es una fecha válida");
      return;
    }

    setValidInputStyle("fecha_term_form");
    resetValidationErrorMessage("tpe_fecha_term_form");

    const contrato = {
      FechaTerminado: fecha.length > 0 ? fecha : null,
      Id: +getElementById("idCon").value
    };

    const modalMulta = new bootstrap.Modal(getElementById('modal-multa'), {});

    try {
      const respuesta = await fetch("/Contrato/Terminar", 
        { 
          method: "post", 
          body: JSON.stringify(contrato), 
          headers: { 
            "Content-Type": "application/json", 
            "Accept": "application/json" 
          } 
        }
      );
      const res = await respuesta.json();

      modalTerminarContrato.hide();
      if (res.error) { mostrarMensaje(false, res.error); return; }
      agregarDatosAlModalMulta(res, contrato.Id);
      modalMulta.show();
      getElementById(getElementById("idFila").value)
      .parentElement
      .parentElement
      .previousElementSibling
      .innerText = aFechaLocal(fecha.length > 0 ? fecha : new Date().toISOString().split("T")[0]);
      cambiarIcono(res.idContrato);
    } catch (e) {
      console.error(e);
      modalTerminarContrato.hide();
      mostrarMensaje(false, "No se pudo terminar el contrato");
    }

  });

  document.querySelectorAll("td .bi-file-earmark-x")?.forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];
      getElementById("idCon").value = `${idFila}`;
      getElementById("idFila").value = e.target.id;
      if (modalTerminarContrato === undefined) 
        modalTerminarContrato = new bootstrap.Modal(getElementById('modal-terminar-contrato'), {});
      modalTerminarContrato.show();
    });
  });

  document.querySelectorAll("td .bi-trash")?.forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Contrato/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

  document.querySelectorAll("td .bi-card-list")?.forEach(i => {
    i.addEventListener("click", verDetalleMulta);
  });
  
  document.querySelectorAll("td .bi-file-earmark-text")?.forEach(i => {
    i.addEventListener("click", async e => {
      const idFila = e.target.id.split("-")[1];
      try {
        const respuesta = await fetch(`/Contrato/Buscar/${idFila}`);
        const contrato = await respuesta.json();console.log(contrato);
        mostrarModalDetalle(contrato);
      } catch (error) {
        mostrarMensaje(false, "No se pudieron cargar los datos");
      }
    });
  });

  function cambiarIcono(idCon) {
    const iconoTerminarContrato = getElementById(`end-${idCon}`);
    const padreIcono = iconoTerminarContrato.parentElement;
    padreIcono.removeChild(iconoTerminarContrato);

    const iconoDetalleMulta = createElement(
      "i", 
      {
        id: `mult-${idCon}`,
      },
      "bi", "bi-card-list", "fs-3", "text-secondary", "icon-hover"
    );
    iconoDetalleMulta.setAttribute("data-bs-toggle", "tooltip");
    iconoDetalleMulta.setAttribute("data-bs-title", "Ver detalle multa");

    tooltipList.push(new bootstrap.Tooltip(iconoDetalleMulta));

    iconoDetalleMulta.addEventListener("click", verDetalleMulta);

    padreIcono.appendChild(iconoDetalleMulta);
  }

  async function verDetalleMulta(e) {
    const idFila = e.target.id.split("-")[1];

    const respuesta = await fetch(`/Contrato/DetalleMulta/${idFila}`);
    const detalleMulta = await respuesta.json();
    if (detalleMulta.error) { 
      mostrarMensaje(false, detalleMulta.error); 
      return; 
    }
    agregarDatosAlModalMulta(detalleMulta, idFila);

    const myModal = new bootstrap.Modal(getElementById('modal-multa'), {});
    myModal.show();
  }

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
});

function mostrarModalDetalle(contrato) {
  const bodyDetalle = getElementById("body-detalle");
  const bodyMensaje = getElementById("body-mensaje");
  if (contrato !== null) {
    removerClases(bodyDetalle, "d-none");
    agregarClases(bodyDetalle, "d-block");
    agregarClases(bodyMensaje, "d-none");
    getElementById("nro").textContent = contrato.id;
    getElementById("propietario").textContent = `${contrato.inmueble.duenio.apellido}, ${contrato.inmueble.duenio.nombre}`;
    getElementById("direccion").textContent = `${contrato.inmueble.calle} ${contrato.inmueble.nroCalle}`;
    getElementById("detalle_tipo").textContent = contrato.inmueble.tipo.tipo;
    getElementById("detalle_uso").textContent = contrato.inmueble.uso;
    getElementById("inquilino").textContent = `${contrato.inquilino.apellido}, ${contrato.inquilino.nombre}`;
    getElementById("fIni").textContent = aFechaLocal(contrato.fechaInicio.split("T")[0]);
    getElementById("fFin").textContent = aFechaLocal(contrato.fechaFin.split("T")[0]);
    getElementById("monto").textContent = contrato.montoMensual;
    getElementById("fTerm").textContent = contrato.fechaTerminado ? aFechaLocal(contrato.fechaTerminado?.split("T")[0]) : " - ";
    const spanContratador = getElementById("contratador");
    const spanTerminador = getElementById("terminador");
    if (spanContratador !== null)
      spanContratador.textContent = `Cod: ${contrato.usuarioContratador.id} - ${contrato.usuarioContratador.apellido}, ${contrato.usuarioContratador.nombre}`;
    if (spanTerminador !== null)
      spanTerminador.textContent = contrato.idUsuarioTerminador ? `Cod: ${contrato.usuarioTerminador.id} - ${contrato.usuarioTerminador.apellido}, ${contrato.usuarioTerminador.nombre}` : " - ";
  } else {
    removerClases(bodyMensaje, "d-none");
    agregarClases(bodyMensaje, "d-block");
    agregarClases(bodyDetalle, "d-none");
  }
  const myModal = new bootstrap.Modal(getElementById('modal_detalle_contrato'), {});
  myModal.show();
}

function agregarDatosAlModalMulta(datos, idContrato) {
  getElementById("mensaje_multa_mesesContrato").textContent = `${datos.cantMesesDelContrato} mes/es`;
  getElementById("mensaje_multa_meses").textContent = `${datos.cantMesesAlquilado} mes/es`;
  getElementById("mensaje_multa_monto").textContent = `$${datos.multa}`;
  getElementById("mensaje_multa_mesesPagos").textContent = `${datos.cantMesesPagados}`;
  getElementById("mensaje_multa_deuda").textContent = `$${datos.deudaDeMesesNoPagados}`;
  getElementById("mensaje_multa_total").textContent = `$${datos.deudaDeMesesNoPagados + datos.multa}`;
  getElementById("mensaje_multa_montoPagado").textContent = `$${datos.multaPaga}`;
  const aPagar = datos.deudaDeMesesNoPagados + datos.multa - datos.multaPaga;
  getElementById("mensaje_multa_aPagar").textContent = `$${aPagar}`;
  const urlFormularioPagoMulta = `/Pago/FormularioPago?idCon=${idContrato}&multa=${aPagar}`;
  const enlacePagarMulta = getElementById("enlacePagarMulta");
  if (+aPagar > 0) {
    removerClases(enlacePagarMulta, "d-none");
    agregarClases(enlacePagarMulta, "d-inline-block");
    enlacePagarMulta.href = urlFormularioPagoMulta;
  } else {
    removerClases(enlacePagarMulta, "d-inline-block");
    agregarClases(enlacePagarMulta, "d-none");
  }
}

function aFechaLocal(fecha) {
  if (!fecha) return;
  return fecha.split("-").reverse().join("/");
}