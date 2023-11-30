document.addEventListener("DOMContentLoaded", function () {
    const aceptarButtons = document.querySelectorAll(".btn-mdl-aceptar");
    const rechazarButtons = document.querySelectorAll(".btn-mdl-rechazar");

    aceptarButtons.forEach((button) => {
        button.addEventListener("click", (event) => {
            const notificationId = event.target.getAttribute("data-notification-id");
            const modalPrincipal = document.getElementById(`modal-${notificationId}`);
            modalPrincipal.style.display = "none";

            const modalSecundaria = document.getElementById(`modal-aceptar-${notificationId}`);
            modalSecundaria.style.display = "block";
        });
    });

    rechazarButtons.forEach((button) => {
        button.addEventListener("click", (event) => {
            const notificationId = event.target.getAttribute("data-notification-id");
            const modalPrincipal = document.getElementById(`modal-${notificationId}`);
            modalPrincipal.style.display = "none";

            const modalSecundaria = document.getElementById(`modal-rechazar-${notificationId}`);
            modalSecundaria.style.display = "block";
        });
    });

    const fechaActual = new Date().toISOString().split("T")[0]; // Obtener la fecha actual en formato "yyyy-mm-dd"

    const fechaPagoTotalPrestamo = document.getElementById("FechaPagoTotalPrestamo");
    fechaPagoTotalPrestamo.addEventListener("input", () => {
        if (fechaPagoTotalPrestamo.value < fechaActual) {
            fechaPagoTotalPrestamo.setCustomValidity("La fecha no puede ser anterior a la fecha actual.");
        } else {
            fechaPagoTotalPrestamo.setCustomValidity("");
        }
    });

    const fechaInicio = document.getElementById("FechaInicio");
    fechaInicio.addEventListener("input", () => {
        if (fechaInicio.value < fechaActual) {
            fechaInicio.setCustomValidity("La fecha no puede ser anterior a la fecha actual.");
        } else {
            fechaInicio.setCustomValidity("");
        }
    });

    const fechaFinalizacion = document.getElementById("FechaFinalizacion");
    fechaFinalizacion.addEventListener("input", () => {
        if (fechaFinalizacion.value < fechaActual) {
            fechaFinalizacion.setCustomValidity("La fecha no puede ser anterior a la fecha actual.");
        } else {
            fechaFinalizacion.setCustomValidity("");
        }
    });

});