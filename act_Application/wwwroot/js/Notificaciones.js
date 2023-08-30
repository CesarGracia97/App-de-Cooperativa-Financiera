document.addEventListener("DOMContentLoaded", function () {
    const modalButtons = document.querySelectorAll(".btn-visualizar");
    const closeModalButtons = document.querySelectorAll(".modal__close");
    const modalSecundariaCloseButtons = document.querySelectorAll(".modal-secundaria__close");
    const confirmButtons = document.querySelectorAll(".btn-mdl-confir");

    modalButtons.forEach((button) => {
        button.addEventListener("click", (event) => {
            const notificationId = event.target.getAttribute("data-notification-id");
            const modal = document.getElementById(`modal-${notificationId}`);
            modal.style.display = "block";
        });
    });

    closeModalButtons.forEach((button) => {
        button.addEventListener("click", (event) => {
            const notificationId = event.target.getAttribute("data-notification-id");
            const modal = document.getElementById(`modal-${notificationId}`);
            modal.style.display = "none";
        });
    });

    modalSecundariaCloseButtons.forEach((button) => {
        button.addEventListener("click", (event) => {
            const notificationId = event.target.getAttribute("data-notification-id");
            const modalSecundaria = document.getElementById(`modal-resp-${notificationId}`);
            modalSecundaria.style.display = "none";
        });
    });

    confirmButtons.forEach((button) => {
        button.addEventListener("click", (event) => {
            const notificationId = event.target.getAttribute("data-notification-id");
            const modalPrincipal = document.getElementById(`modal-${notificationId}`);
            modalPrincipal.style.display = "none";

            const modalSecundaria = document.getElementById(`modal-resp-${notificationId}`);
            modalSecundaria.style.display = "block";
        });
    });

    // Cerrar modal si se hace clic fuera del contenido de la modal
    window.addEventListener("click", (event) => {
        if (event.target.classList.contains("modal")) {
            event.target.style.display = "none";
            const notificationId = event.target.getAttribute("id").replace("modal-", "");
            const modalSecundaria = document.getElementById(`modal-resp-${notificationId}`);
            modalSecundaria.style.display = "none";
        }
    });

});