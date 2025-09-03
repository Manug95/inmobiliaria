import { getElementById, createElement, agregarClases } from "./frontUtils.js";

export function renderizarTabla(datos, rutas) {
  // getElementById("titulo").textContent = "Stock disponible en " + depositoSeleccionado;
  const tabla = getElementById("cuerpo");
  tabla.innerHTML = "";

  datos.forEach(d => {
    const fila = createElement("tr", { id: d.id !== undefined ? d.id : d.Id });
    const columnas = {};

    Object.keys(d).forEach(key => {
      if (key !== "id" && key !== "Id") {
        columnas[key] = createElement("td", { content: d[key].toString() }, "align-middle", "text-center", "text-break");
        // fila.appendChild(createElement("td", { content: d[key].toString() }, "align-middle", "text-center", "text-break"));
      }
    });
    const direccion = createElement("td", { content: columnas.calle.toString() + columnas.nroCalle.toString() }, "align-middle", "text-center", "text-break");
    const duenio = createElement("td", { content: columnas.duenio.apellido.toString() + ", " + columnas.duenio.nombre.toString() }, "align-middle", "text-center", "text-break");
    //direccion, tipo, cant amb, dueño disp, precio
    fila.appendChild(direccion);
    fila.appendChild(columnas.tipo);
    fila.appendChild(columnas.cantidadAmbientes);
    fila.appendChild(duenio);
    fila.appendChild(columnas.precio);

    const divIconos = createElement("div", {}, "d-flex", "justify-content-evenly", "align-items-center", "contenedor-iconos");

    const iconoActualizar = createElement("i", {}, "bi", "bi-pencil-square", "fs-3", "icon-hover");//data-bs-toggle="tooltip" data-bs-title="Editar"
    iconoActualizar.setAttribute("data-bs-toggle", "tooltip");
    iconoActualizar.setAttribute("data-bs-title", "Editar");
    const anchorDelIconoActualizar = createElement(
      "a", 
      { 
        content: iconoActualizar,
        href: `${rutas.actualizar}/${d.id}` 
      },
      "icon-enlace"
    );
    const iconoBorrar = createElement(
      "i", 
      {
        id: `del-${d.id}`
      },
      "bi", "bi-trash", "fs-3", "text-danger", "icon-hover"
    );
    iconoBorrar.setAttribute("data-bs-toggle", "tooltip");
    iconoBorrar.setAttribute("data-bs-title", "Eliminar");

    const tdAcciones = createElement("td", { }, "ps-3", "align-middle", "text-center");
    divIconos.appendChild(anchorDelIconoActualizar);
    divIconos.appendChild(iconoBorrar);
    tdAcciones.appendChild(divIconos);
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