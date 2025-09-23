import { getElementById, mostrarMensaje, getFormInputValue } from "./frontUtils.js";
import {
  resetValidationErrorMessage,
  resetValidationInputStyle,
  setValidationErrorMessage,
  setInvalidInputStyle,
  setValidInputStyle,
  validarFechaDeInputDate,
  validarPrecio,
  validarDescripcionTipoInmueble,
  validarFormSelect
} from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {

  mostrarMensaje(false, null);

  const form = getElementById("form-pago");
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
    ["Fecha", validarFechaDeInputDate],
    ["Importe", validarPrecio],
    ["Detalle", validarDescripcionTipoInmueble],
    ["Tipo", validarFormSelect]
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
    Fecha: getFormInputValue("Fecha"),
    Importe: getFormInputValue("Importe"),
    Detalle: getFormInputValue("Detalle"),
    Tipo: getFormInputValue("Tipo"),
    Id: getFormInputValue("Id"),
    IdContrato: getFormInputValue("IdContrato")
  };

  return inputsValues;
}
