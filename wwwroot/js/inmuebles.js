import { getElementById, mostrarMensaje, mostrarPregunta } from "./frontUtils.js";


document.addEventListener("DOMContentLoaded", () => {

  mostrarMensaje(false, null);

  const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
  const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
  
  document.querySelectorAll(".bi-trash").forEach(i => {
    i.addEventListener("click", e => {
      const idFila = e.target.id.split("-")[1];

      getElementById("btn_si").href = `/Inmueble/Eliminar/${idFila}`;

      mostrarPregunta(null);
    });
  });

});
