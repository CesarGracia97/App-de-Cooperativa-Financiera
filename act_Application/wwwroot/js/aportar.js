const form = document.getElementById("aportacion-form");
const cuentaInput = document.getElementById("cuenta");
const errorMessage = document.querySelector(".error-message");

cuentaInput.addEventListener("input", function () {
    if (cuentaInput.validity.valid) {
        errorMessage.style.display = "none";
        cuentaInput.style.borderColor = "#ccc";
    } else {
        errorMessage.style.display = "block";
        cuentaInput.style.borderColor = "red";
    }
});

form.addEventListener("submit", function (event) {
    event.preventDefault();
    // Aquí puedes agregar el código para guardar los datos del formulario
});

const clearButton = document.querySelector(".clear-button");
clearButton.addEventListener("click", function () {
    form.reset();
    errorMessage.style.display = "none";
    cuentaInput.style.borderColor = "#ccc";
});
