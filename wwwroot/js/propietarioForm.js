// import { enviarPOST } from "./httpRequests.js";
import { getElementById, getFormInputValue, mostrarMensaje } from "./frontUtils.js";
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

document.addEventListener("DOMContentLoaded", (e) => {
  const form = getElementById("form-propietario");

  form?.addEventListener("submit", (e) => {
    e.preventDefault();
    const formValues = getFormValues();
    
    if (validarFormulario(formValues)) {
      form.submit();
    }

    return;

  });

  document.querySelectorAll("td .bi-pencil-square").forEach(i => {
    i.addEventListener("click", e => {
      // const idFIla = e.target.id.split("-")[1];
      // const fila = getElementById(idFIla);
      const fila = e.target.parentElement.parentElement.parentElement;
      const datosFila = getRawValues(fila);

      getElementById("nombre").value = datosFila.nombre;
      getElementById("apellido").value = datosFila.apellido;
      getElementById("dni").value = datosFila.dni;
      getElementById("telefono").value = datosFila.telefono;
      getElementById("email").value = datosFila.email;
      getElementById("Id").value = datosFila.id;

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
      
      const myModal = new bootstrap.Modal(getElementById('modal_formulario_propietario'), {});
      myModal.show();
      // console.log(fila);
    });
  });

});

//para cuando explique como recibir peticioens ajax
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
    // ["email", (_) => false],
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