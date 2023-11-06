// Mostrar el modal si hay un mensaje de error
document.addEventListener('DOMContentLoaded', function () {
    var modal = document.getElementById("myModal");
    var span = document.getElementsByClassName("close")[0];

    span.onclick = function () {
        modal.style.display = "none";
    };

    var loginButton = document.querySelector('.submit');
    loginButton.addEventListener('click', function (event) {
        var emailInput = document.getElementById("email");
        var errorMessage = "Caracteres especiales o formato de correo incorrecto detectado, corríjalo por favor.";

        if (errorMessage && !isValidEmail(emailInput.value)) {
            event.preventDefault();
            var modalContent = modal.querySelector(".modal-content");
            modalContent.querySelector("p").textContent = errorMessage;
            modal.style.display = "block";
        }
    });

    function isValidEmail(email) {
        return /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/.test(email);
    }
});