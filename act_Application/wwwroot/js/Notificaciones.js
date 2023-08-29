document.addEventListener("DOMContentLoaded", function () {
    const modalButtons = document.querySelectorAll(".btn-visualizar");
    const closeModalButtons = document.querySelectorAll(".modal__close");

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
    // Cerrar modal si se hace clic fuera del contenido de la modal
    window.addEventListener("click", (event) => {
        if (event.target.classList.contains("modal")) {
            event.target.style.display = "none";
        }
    });
});