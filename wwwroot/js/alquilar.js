import { getElementById, getFormInputValue } from "./frontUtils.js";
import {
  resetValidationErrorMessage,
  setInvalidInputStyle,
  setValidInputStyle,
  setValidationErrorMessage,
  validarFormSelect,
  validarPrecio,
  validarFechaDeInputDate,
  validarFechaInicioDelContrato,
  validarFechaFinDelContrato,
  validarCantidadAmbientes
} from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {
  const form = getElementById("form-buscar-inmuebles");

  form?.addEventListener("submit", (e) => {
    e.preventDefault();

    const formValues = getFormValues();

    if (validarFormulario(formValues)) {
      if (validarRangosDeFechas(formValues)) 
        setTimeout((form) => { form.submit(); }, 500, form);
    }

    return;

  });

  getElementById("buscador")
  ?.addEventListener("input", debounce((e) => {
      buscar(e.target.value);
    }, 500)
  );

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));

});

function getFormValues() {
  const inputsValues = {
    IdTipoInmueble: getFormInputValue("IdTipoInmueble"),
    Uso: getFormInputValue("Uso"),
    CantidadAmbientes: getFormInputValue("CantidadAmbientes"),
    Precio: getFormInputValue("Precio"),
    FechaInicio: getFormInputValue("FechaInicio"),
    FechaFin: getFormInputValue("FechaFin")
  };

  return inputsValues;
}

/**
 * @param {*} values 
 * @returns {Boolean}
 */
function validarFormulario(values) {
  const mapaValidador = new Map([
    ["FechaInicio", validarFechaDeInputDate],
    ["FechaFin", validarFechaDeInputDate],
    ["IdTipoInmueble", validarFormSelect],
    ["CantidadAmbientes", validarCantidadAmbientes],
    ["Uso", validarFormSelect],
    ["Precio", validarPrecio]
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

function validarRangosDeFechas(formValues) {
  const mapaValidador = new Map([
    ["FechaInicio", validarFechaInicioDelContrato],
    ["FechaFin", validarFechaFinDelContrato]
  ]);

  const { FechaInicio, FechaFin } = formValues;

  return Object.keys(formValues)
    .map(v => {
      const fnValidadora = mapaValidador.get(v);
      if (fnValidadora === undefined) return true;
      const result = fnValidadora(FechaInicio, FechaFin);
      if (result !== undefined) { setInvalidInputStyle(v); setValidationErrorMessage(`tpe_${v}`, result.errorMessage); }
      else { setValidInputStyle(v); resetValidationErrorMessage(`tpe_${v}`); }
      return result === undefined;
    })
    .every(v => v)
  ;
}