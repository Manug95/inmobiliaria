import { getElementById, createElement, agregarClases } from "./frontUtils.js";

export function renderizarTabla(datos, rutas) {
  // getElementById("titulo").textContent = "Stock disponible en " + depositoSeleccionado;
  const tabla = getElementById("cuerpo");
  tabla.innerHTML = "";

  datos.forEach(d => {
    const fila = createElement("tr", { id: d.id !== undefined ? d.id : d.Id });

    Object.keys(d).forEach(key => {
      if (key !== "id" && key !== "Id") {
        fila.appendChild(createElement("td", { content: d[key].toString() }, "align-middle", "text-center", "text-break"));
      }
    });

    const tdAcciones = createElement("td", { colSpan: "2" }, "ps-3", "align-middle", "text-center");
    const iconoActualizar = createElement(
      "a", 
      { 
        content: createElement("i", {}, "bi", "bi-pencil-square", "fs-3"), 
        href: rutas.actualizacion + d.id 
      }
    );
    const iconoBorrar = createElement(
      "a", 
      { 
        content: createElement("i", {}, "bi", "bi-trash", "fs-3"), 
        href: rutas.eliminacion + d.id 
      }
    );
    tdAcciones.appendChild(iconoActualizar);
    tdAcciones.appendChild(iconoBorrar);
    fila.appendChild(tdAcciones);

    tabla.appendChild(fila);
  });
  
}

export function crearFilaMensajeDeTabla(mensaje, tabla = getElementById("cuerpo"), redistribuible = false) {
  tabla.innerHTML = "";
  const fila = createElement("tr", {});
  const td = createElement("td", { content: mensaje, colSpan: redistribuible ? "8" : "7" }, "align-middle", "text-center");
  fila.appendChild(td);
  tabla.appendChild(fila);
}

export function crearFilaMensaje({ mensaje, idTabla, cantColumnas }) {
  const tabla = getElementById(idTabla);
  tabla.innerHTML = "";
  const fila = createElement("tr", {});
  const td = createElement("td", { content: mensaje, colSpan: cantColumnas }, "align-middle", "text-center");
  fila.appendChild(td);
  tabla.appendChild(fila);
}