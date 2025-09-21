import { getElementById, mostrarMensaje, getFormInputValue } from "./frontUtils.js";
import {
  resetValidationErrorMessage,
  setValidationErrorMessage,
  setInvalidInputStyle,
  setValidInputStyle,
  validarNombreApellido,
  validarEmail,
  validarFormSelect
} from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {

  mostrarMensaje(false, null);

  const form = getElementById("form-usuario");
  form.addEventListener("submit", async e => {
    e.preventDefault();

    const formValues = getFormValues();

    if (validarFormulario(formValues)) {
      setTimeout((form) => { form.submit(); }, 500, form);
    }

    return;
  });

});

/**
 * @param {*} values 
 * @returns {Boolean}
 */
function validarFormulario(values) {
  const mapaValidador = new Map([
    ["Nombre", validarNombreApellido],
    ["Apellido", validarNombreApellido],
    ["Email", validarEmail],
    ["Rol", validarFormSelect],
    ["Password", validarPassword]
  ]);

  const esValido = Object.keys(values)
    .map(v => {
      const fnValidadora = mapaValidador.get(v);
      if (fnValidadora === undefined) return true;
      const result = fnValidadora(values[v]);
      if (result !== undefined) { setInvalidInputStyle(v); setValidationErrorMessage(`tpe_${v}`, result.errorMessage); }
      else { setValidInputStyle(v); resetValidationErrorMessage(`tpe_${v}`); }
      return result === undefined;
    })
    .every(v => v);

  return esValido;
}

function getFormValues() {
  const inputsValues = {
    Nombre: getFormInputValue("Nombre"),
    Apellido: getFormInputValue("Apellido"),
    Email: getFormInputValue("Email"),
    Rol: getFormInputValue("Rol"),
    Password: getFormInputValue("Password")
  };

  return inputsValues;
}

function validarPassword(password) {
  if (!password) {
    return { errorMessage: "La contraseña es requerida" };
  }

  // if (password.length < 6 || password.length > 25) {
  //   return { errorMessage: "" };
  // } 

  return;
}
