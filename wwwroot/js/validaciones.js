import { agregarClases, removerClases, getElementById } from "./frontUtils.js";

export function setValidInputStyle(id) {
  const input = getElementById(id);
  if (input !== null) {
    removerClases(input, "is-invalid");
    agregarClases(input, "is-valid");
  }
  // input.dispatchEvent(new Event("change", { bubbles: true }));
}

export function setInvalidInputStyle(id) {
  const input = getElementById(id);
  if (input !== null) {
    removerClases(input, "is-valid");
    agregarClases(input, "is-invalid");
  }
  // input.dispatchEvent(new Event("change", { bubbles: true }));
}

export function resetValidationInputStyle(id) {
  const input = getElementById(id);
  if (input !== null) {
    removerClases(input, "is-valid");
    removerClases(input, "is-invalid");
  }
}

export function setValidationErrorMessage(tooltipId, message) {
  const tooltip = getElementById(tooltipId);
  if (tooltip !== null) tooltip.innerText = message;
}

export function resetValidationErrorMessage(tooltipId) {
  const tooltip = getElementById(tooltipId);
  if (tooltip !== null) tooltip.innerText = "";
}

export function validarFormSelect(value) {
  if (value === "") {
    return { errorMessage: "El campo es requerido" };
  }
  return;
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

// export function validarDNI(dni) {
//   if (!dni) return false;

//   const regex = /^\d{8}$/;
//   if (!regex.test(dni)) return false;

//   return true;
// }

export function validarDNI(dni) {
  if (dni === null) return { errorMessage: "El DNI es requerido" };

  if (typeof dni !== "string") return { errorMessage: "Debe ser un número de 7 u 8 dígitos numéricos" };

  if (dni.length < 7 || dni.length > 8) return { errorMessage: "Debe ser un número de 7 u 8 dígitos" };

  const regex = /^\d{7,8}$/;
  if (!regex.test(dni)) return { errorMessage: "El DNI ingresado NO es valido" };

  return;
}

/**
 * @param {string} cadena 
 */
export function validarNombreApellido(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido" };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto" };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido" };
  if (cadena.length > 25) return { errorMessage: "El máximo de caracteres de de 25" };
  return;
}

export function validarTelefono(cadena) {
  // if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido." };
  // if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto." };
  // if (cadena.trim() === "") return { errorMessage: "El campo es requerido." };
  // if (cadena.length > 20) return { errorMessage: "El máximo de caracteres de de 20." };

  // const regExp = /^(\+54)?\s?(11|15|2\d{3}|3[1-9]\d{2}|4\d{3}|5[1-3]|6[1-8]|7[1-5]|8[1-7]|9[1-8])?\s?(\d{4}[ -]?\d{4}|\d{2}[ -]?\d{4}[ -]?\d{4})$/;
  // const regExp = /^(?:(?:(?:\\+|00)?54(?:\\s)?)?(?:(?:9)(?:\\s)?)?(?:(?:0)?(?:\\s)?)?(?:(?:\\(?11\\)?)|(?:\\(?2\\d{2,3}\\)?)|(?:\\(?3\\d{2,3}\\)?)|(?:\\(?6\\d{2}\\)?)|(?:\\(?8\\d{2}\\)?)))(?:(?:\\s)?(?:15)(?:\\s)?)?(?|(\\d{2})|(\\d{3})|(\\d{4})|(\\d{5}))(?:(?:\\s|\\-|\\.)?)(?:(?:\\d{4})|(?|(\\d{4})|(\\d{3})|(\\d{2})|(\\d{1})))(?:(?:\\s|\\-|\\.)?)(?:(?:\\d{4})|(?|(\\d{4})|(\\d{3})|(\\d{2})|(\\d{1})))$/;
  // const regExp = /^\d{4}-\d{6}$/;
  // const regExp = /^(?:\+54)?\s*(?:\(?(?:11|2\d{2,3}|3\d{2,3}|6\d{2}|8\d{2})\)?)\s*(?:15\s*)?\d{4}[-\s.]?\d{4}$/;
  const regExp = /^(\+54\s)?0?(\d{2,4})\s(15\s)?(\d{4}-?\d{4})|(\d{3}-?\d{4})|(\d{2}-?\d{4})$/;
  // /(\d{4}-?\d{4})|(\d{3}-?\d{4})|(\d{2}-?\d{4})/ //1234–5678 o 123–4567 o 12-3456
  if (!regExp.test(cadena)) return { errorMessage: "El teléfono ingresado NO es valido" };
  return;
}

export function validarEmail(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido" };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto" };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido" };
  if (cadena.length > 50) return { errorMessage: "El máximo de caracteres de de 50" };
  const regExp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  if (!regExp.test(cadena)) return { errorMessage: "El EMAIL ingresado NO es valido" };
  return;
}

/**
 * @param {string} cadena 
 */
export function validarNombreTipoInmueble(cadena) {
  if (cadena === undefined || cadena === null) return { errorMessage: "El campo es requerido" };
  if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto" };
  if (cadena.trim() === "") return { errorMessage: "El campo es requerido" };
  if (cadena.length > 25) return { errorMessage: "El máximo de caracteres de de 25" };
  return;
}

export function validarDescripcionTipoInmueble(cadena) {
  if (cadena !== undefined && cadena !== null) {
    if (typeof cadena !== "string") return { errorMessage: "El campo de ser de texto" };
  }
  if (cadena.length > 255) return { errorMessage: "El máximo de caracteres de de 255" };
  return;
}

export function validarNroCalle(cadena) {
  // const reNroCalle = /^\d{1,5}[A-Za-z]?$/;
  if (cadena.length === 0) {
    return { errorMessage: "El número de calle esta vacío o es incorrecto" };
  }
  if (isNaN(parseInt(cadena[0]))) {
    return { errorMessage: "El número de calle debe comenzar con un número" };
  }
  const num = parseInt(cadena);
  if (isNaN(num)) return { errorMessage: "No es un número" };
  if (num <= 0) {
    return { errorMessage: "El número de calle debe ser mayor a 0" };
  }
  if (num > 99999) {
    return { errorMessage: "El número de calle es demasiado grande" };
  }
  if (cadena.length > 1) {
    const ultimo = cadena[cadena.length - 1];
    if (isNaN(ultimo) && !(/[a-zA-Z]/.test(ultimo))) {
      return { errorMessage: "El número de calle solo puede terminar en una letra opcional" };
    }
  }
  return;
}

export function validarCalle(cadena) {
  // const reCalle = /^[A-Za-zÁÉÍÓÚáéíóúÑñ\s\.\-]{3,100}$/;
  if (cadena.length === 0) {
    return { errorMessage: "La calle es obligatoria" };
  }
  // if (cadena.length < 3) {
  //   return { errorMessage: "La calle debe tener al menos 3 caracteres" };
  // }
  if (cadena.length > 50) {
    return { errorMessage: "La calle no puede superar los 50 caracteres" };
  }
  for (let c of cadena) {
    if (!(/[1-9a-zA-ZáéíóúÁÉÍÓÚñÑ\s\.\-°]/.test(c))) {
      return { errorMessage: "La calle contiene caracteres inválidos" };
    }
  }
  return;
}

export function validarCantidadAmbientes(cadena) {
  if (cadena.length === 0) {
    return { errorMessage: "La cantidad de ambientes es obligatoria" };
  }
  if (isNaN(cadena)) {
    return { errorMessage: "La cantidad de ambientes debe ser un número" };
  }
  const cantidad = parseInt(cadena, 10);

  if (!Number.isInteger(cantidad) || (cantidad < Number.parseFloat(cadena))) {
    return { errorMessage: "La cantidad de ambientes debe ser un número entero" };
  }
  if (cantidad <= 0) {
    return { errorMessage: "La cantidad de ambientes debe ser mayor que 0" };
  }
  if (cantidad > 100) {
    return { errorMessage: "La cantidad de ambientes no puede superar los 100" };
  }
  return;
}

export function validarPrecio(cadena) {
  // const rePrecio = /^\d+([.,]\d{1,2})?$/;
  if (cadena.length === 0) {
    return { errorMessage: "El precio es obligatorio" };
  }
  let precioNum = parseFloat(cadena.replace(",", "."));
  if (isNaN(precioNum)) {
    return { errorMessage: "El precio debe ser un número" };
  }
  if (precioNum <= 0) {
    return { errorMessage: "El precio debe ser mayor que 0" };
  }
  return;
}

export function validarLatitud(cadena) {
  // const reLat = /^-?([0-8]?\d(\.\d+)?|90(\.0+)?)$/;
  if (cadena.length === 0) {
    return { errorMessage: "La latitud es obligatoria" };
  }
  let latNum = parseFloat(cadena);
  if (isNaN(latNum)) {
    return { errorMessage: "La latitud debe ser un número" };
  }
  if (latNum < -90 || latNum > 90) {
    return { errorMessage: "La latitud debe estar entre -90 y 90" };
  }
  return;
}

export function validarLongitud(cadena) {
  // const reLng = /^-?(1[0-7]\d(\.\d+)?|180(\.0+)?|\d{1,2}(\.\d+)?)$/;
  if (cadena.length === 0) {
    return { errorMessage: "La longitud es obligatoria" };
  }
  let lngNum = parseFloat(cadena);
  if (isNaN(lngNum)) {
    return { errorMessage: "La longitud debe ser un número" };
  }
  if (lngNum < -180 || lngNum > 180) {
    return { errorMessage: "La longitud debe estar entre -180 y 180" };
  }
  return;
}

export function validarFechaDeInputDate(cadena) {
  if (!cadena) return { errorMessage: "Fecha incompleta o incorrecta" };
  return;
}

export function validarFechaInicioDelContrato(inico, fin, terminacion) {
  const fechaIni = new Date(inico);
  const fechaAct = new Date();
  
  if (fechaIni.getFullYear() < fechaAct.getFullYear()) return { errorMessage: "Fecha inicio no puede ser menor a la fecha actual" };
  else if (fechaIni.getMonth() < fechaAct.getMonth()) return { errorMessage: "Fecha inicio no puede ser menor a la fecha actual" };
  else if (fechaIni.getDate() < fechaAct.getDate()) return { errorMessage: "Fecha inicio no puede ser menor a la fecha actual" };
    
  const msFechaIni = fechaIni.getTime();
  const msFechaFIn = new Date(fin).getTime();

  if (msFechaIni > msFechaFIn) return { errorMessage: "Fecha inicio no puede ser mayor a fecha fin del contrato" };
  if (terminacion) {
    const fechaTerm = new Date(terminacion).getTime();
    if (msFechaIni > fechaTerm) return { errorMessage: "Fecha inicio no puede ser mayor a fecha terminacion del contrato" };
  }

  return;
}

export function validarFechaFinDelContrato(inico, fin, terminacion) {
  const fechaIni = new Date(inico).getTime();
  const fechaFIn = new Date(fin).getTime();

  if (fechaFIn < fechaIni) return { errorMessage: "Fecha fin no puede ser menor a fecha incio del contrato" };
  if (terminacion) {
    const fechaTerm = new Date(terminacion).getTime();
    if (fechaFIn > fechaTerm) return { errorMessage: "Fecha fin no puede ser mayor a fecha terminacion del contrato" };
  }

  return;
}

export function validarFechaTerminacionDelContrato(inico, fin, terminacion) {
  if (terminacion) {
    const fechaIni = new Date(inico).getTime();
    const fechaFIn = new Date(fin).getTime();
    const fechaTerm = new Date(terminacion).getTime();

    if (fechaTerm < fechaIni || fechaTerm < fechaFIn) return { errorMessage: "Fecha terminacion no puede ser menor a las fechas de inicio o fin del contrato" };
  }

  return;
}