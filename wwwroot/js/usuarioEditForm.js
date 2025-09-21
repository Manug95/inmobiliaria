import { getElementById, mostrarMensaje, getFormInputValue } from "./frontUtils.js";
import {
  resetValidationErrorMessage,
  setValidationErrorMessage,
  setInvalidInputStyle,
  setValidInputStyle,
  validarEmail,
  validarFormSelect
} from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {

  mostrarMensaje(false, null);

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

  const formPersonal = getElementById("form-datos-personales");
  formPersonal.addEventListener("submit", async e => {
    e.preventDefault();

    const formValues = getFormPersonalValues();

    if (validarFormularioPersonal(formValues)) {
      setTimeout((form) => { form.submit(); }, 500, formPersonal);
    }

    return;
  });

  const formSesion = getElementById("form-usuario");
  formSesion.addEventListener("submit", async e => {
    e.preventDefault();

    const formValues = getFormSesionValues();

    if (validarFormularioSesion(formValues)) {
      setTimeout((form) => { form.submit(); }, 500, formSesion);
    }

    return;
  });

});

/**
 * @param {*} values 
 * @returns {Boolean}
 */
function validarFormularioPersonal(values) {
  const mapaValidador = new Map([
    ["Nombre", validarNombreApellido],
    ["Apellido", validarNombreApellido]
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

function validarNombreApellido(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido" };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto" };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido" };
  if (cadena.length > 100) return { errorMessage: "El máximo de caracteres de de 50" };
  return;
}

/**
 * @param {*} values 
 * @returns {Boolean}
 */
function validarFormularioSesion(values) {
  const mapaValidador = new Map([
    ["Email", validarEmail],
    ["Rol", validarFormSelect]
    // ,["Password", validarPassword]
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

function getFormPersonalValues() {
  const inputsValues = {
    Nombre: getFormInputValue("Nombre"),
    Apellido: getFormInputValue("Apellido")
  };

  return inputsValues;
}

function getFormSesionValues() {
  const inputsValues = {
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
