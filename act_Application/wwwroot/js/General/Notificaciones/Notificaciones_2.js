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
});