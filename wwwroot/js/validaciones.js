import { agregarClases, removerClases, getElementById } from "./frontUtils.js";

export function setValidInputStyle(id) {
  const input = getElementById(id);
  removerClases(input, "is-invalid");
  agregarClases(input, "is-valid");
}

export function setInvalidInputStyle(id) {
  const input = getElementById(id);
  removerClases(input, "is-valid");
  agregarClases(input, "is-invalid");
}

export function resetValidationInputStyle(id) {
  const input = getElementById(id);
  removerClases(input, "is-valid");
  removerClases(input, "is-invalid");
}

export function setValidationErrorMessage(tooltipId, message) {
  const tooltip = getElementById(tooltipId);
  tooltip.innerText = message;
}

export function resetValidationErrorMessage(tooltipId) {
  const tooltip = getElementById(tooltipId);
  tooltip.innerText = "";
}

export function validarFormSelect(value) {
  if (value === "") {
    return false;
  }
  return true;
}

export function validarFecha(fecha) {
  if (!fecha) return false;

  let fechaSel = new Date(fecha);
  if (!fechaSel) return false;
  fechaSel = fechaSel.getTime();

  const fechaAct = new Date().getTime();
  if (fechaSel > fechaAct) return false;
  
  return true;
}

export function validarInputNumberPositivo(value) {
  if (!value) return false;
  value += "";

  if (value.includes(".") || value.includes("+") || value.includes("e") || value.includes("-")) return false;

  const num = Number.parseInt(value);
  if (!num) return false
  if (num < 0) return false

  return true;
}

function validarMotivo(motivo) {
  // if (!motivo) return false;
  // if (motivo.length > 100) return false;

  // return true;
  return validarFormSelect(motivo);
}

// export function validarDNI(dni) {
//   if (!dni) return false;

//   const regex = /^\d{8}$/;
//   if (!regex.test(dni)) return false;

//   return true;
// }

export function validarDNI(dni) {
  if (dni === null) return { errorMessage: "El DNI es requerido." };

  if (typeof dni !== "string") return { errorMessage: "El DNI debe ser un número de 8 dígitos numéricos." };

  if (dni.length < 8 || dni.length > 8) return { errorMessage: "El DNI debe ser un número de 8 dígitos." };

  const regex = /^\d{8}$/;
  if (!regex.test(dni)) return { errorMessage: "El DNI ingresado NO es valido." };

  return;
}

/**
 * @param {string} cadena 
 */
export function validarNombreApellido(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido." };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto." };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido." };
  if (cadena.length > 25) return { errorMessage: "El máximo de caracteres de de 25." };
  return;
}

export function validarTelefono(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido." };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto." };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido." };
  if (cadena.length > 15) return { errorMessage: "El máximo de caracteres de de 15." };
  // const regExp = /^(\+54)?\s?(11|15|2\d{3}|3[1-9]\d{2}|4\d{3}|5[1-3]|6[1-8]|7[1-5]|8[1-7]|9[1-8])?\s?(\d{4}[ -]?\d{4}|\d{2}[ -]?\d{4}[ -]?\d{4})$/;
  // const regExp = /^(?:(?:(?:\\+|00)?54(?:\\s)?)?(?:(?:9)(?:\\s)?)?(?:(?:0)?(?:\\s)?)?(?:(?:\\(?11\\)?)|(?:\\(?2\\d{2,3}\\)?)|(?:\\(?3\\d{2,3}\\)?)|(?:\\(?6\\d{2}\\)?)|(?:\\(?8\\d{2}\\)?)))(?:(?:\\s)?(?:15)(?:\\s)?)?(?|(\\d{2})|(\\d{3})|(\\d{4})|(\\d{5}))(?:(?:\\s|\\-|\\.)?)(?:(?:\\d{4})|(?|(\\d{4})|(\\d{3})|(\\d{2})|(\\d{1})))(?:(?:\\s|\\-|\\.)?)(?:(?:\\d{4})|(?|(\\d{4})|(\\d{3})|(\\d{2})|(\\d{1})))$/;
  const regExp = /^\d{4}-\d{6}$/;
  if (!regExp.test(cadena)) return { errorMessage: "El teléfono ingresado NO es valido." };
  return;
}

export function validarEmail(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido." };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto." };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido." };
  if (cadena.length > 50) return { errorMessage: "El máximo de caracteres de de 50." };
  const regExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  if (!regExp.test(cadena)) return { errorMessage: "El EMAIL ingresado NO es valido." };
  return;
}