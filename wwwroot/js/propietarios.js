import { getElementById, mostrarMensaje } from "./frontUtils.js";
import { resetValidationInputStyle, resetValidationErrorMessage } from "./validaciones.js";


document.addEventListener("DOMContentLoaded", () => {

  getElementById("btn_add").addEventListener("click", e => {
    getElementById("nombre").value = "";
    getElementById("apellido").value = "";
    getElementById("dni").value = "";
    getElementById("telefono").value = "";
    getElementById("email").value = "";
    getElementById("Id").value = 0;

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

    const modalForm = getElementById('modal_formulario_propietario');
    const myModal = new bootstrap.Modal(modalForm, {});
    myModal.show();
  });

  mostrarMensaje(false, null);
  // const modalForm = getElementById('modal_formulario_propietario');
  // const errAttr = modalForm.getAttribute("data-er");
  // if (errAttr === "true") {
  //   const myModal = new bootstrap.Modal(modalForm, {});
  //   myModal.show();
  // }

});
