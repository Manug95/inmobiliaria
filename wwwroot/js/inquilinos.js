import { agregarClases, getElementById, getFormInputValue, mostrarMensaje, mostrarPregunta, removerClases } from "./frontUtils.js";
import { 
  setInvalidInputStyle, 
  setValidInputStyle, 
  validarNombreApellido, 
  validarDNI, 
  validarTelefono, 
  validarEmail, 
  resetValidationInputStyle ,
  setValidationErrorMessage,
  resetValidationErrorMessage
} from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {
  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

  getElementById("btn_add").addEventListener("click", e => {
    getElementById("nombre").value = "";
    getElementById("apellido").value = "";
    getElementById("dni").value = "";
    getElementById("telefono").value = "";
    getElementById("email").value = "";
    getElementById("Id").value = 0;

    resetValidationStatus();

    const modalForm = getElementById('modal_formulario_inquilino');
    const myModal = new bootstrap.Modal(modalForm, {});
    myModal.show();
  });

  document.querySelectorAll("td .bi-pencil-square").forEach(i => {
    i.addEventListener("click", e => {
      const idFIla = e.target.id.split("-")[1];
      const fila = getElementById(idFIla);
      const datosFila = getRawValues(fila);

      getElementById("nombre").value = datosFila.nombre;
      getElementById("apellido").value = datosFila.apellido;
      getElementById("dni").value = datosFila.dni;
      getElementById("telefono").value = datosFila.telefono;
      getElementById("email").value = datosFila.email;
      getElementById("Id").value = datosFila.id;

      resetValidationStatus();
      
      const myModal = new bootstrap.Modal(getElementById('modal_formulario_inquilino'), {});
      myModal.show();
    });
  });

  document.querySelectorAll("td .bi-trash").forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Inquilino/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

  mostrarMensaje(false, null);

  const form = getElementById("form-inquilino");

  form?.addEventListener("submit", (e) => {
    e.preventDefault();
    const formValues = getFormValues();
    
    if (validarFormulario(formValues)) {
      setTimeout((form) => { form.submit(); }, 500, form);
    }

    return;

  });

});


// async function enviar(formValues) {
//   const respuesta = await enviarPOST("/vacunacion", formValues);
//   mostrarMensaje(respuesta.ok, respuesta.mensaje ?? "Vacunación Registrada");
// }

function getFormValues() {
  return {
    nombre: getFormInputValue("nombre"),
    apellido: getFormInputValue("apellido"),
    dni: getFormInputValue("dni"),
    telefono: getFormInputValue("telefono"),
    email: getFormInputValue("email")
  };
}

/**
 * @param {HTMLTableRowElement} raw 
 */
function getRawValues(raw) {
  const datos = raw.children;
  return {
    apellido: datos.item(0).innerText.trim(),
    nombre: datos.item(1).innerText.trim(),
    dni: datos.item(2).innerText.trim(),
    telefono: datos.item(3).innerText.trim(),
    email: datos.item(4).innerText.trim(),
    id: raw.id
  };
}

/**
 * @param {*} values 
 * @returns {Boolean}
 */
function validarFormulario(values) {
  const mapaValidador = new Map([
    ["nombre", validarNombreApellido],
    ["apellido", validarNombreApellido],
    ["dni", validarDNI],
    ["telefono", validarTelefono],
    ["email", validarEmail]
  ]);

  return Object.keys(values)
  .map(v => {
    const result = mapaValidador.get(v)(values[v]);
    if (result !== undefined) { setInvalidInputStyle(v); setValidationErrorMessage(`tpe_${v}`, result.errorMessage); } 
    else { setValidInputStyle(v); resetValidationErrorMessage(`tpe_${v}`); }
    return result === undefined;
  })
  .every(v => v);
}

function resetValidationStatus() {
    resetValidationInputStyle("nombre");
    resetValidationInputStyle("apellido");
    resetValidationInputStyle("dni");
    resetValidationInputStyle("telefono");
    resetValidationInputStyle("email");

    resetValidationErrorMessage("tpe_nombre");
    resetValidationErrorMessage("tpe_apellido");
    resetValidationErrorMessage("tpe_dni");
    resetValidationErrorMessage("tpe_telefono");
    resetValidationErrorMessage("tpe_email");
}
