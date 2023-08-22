const clearButton = document.querySelector(".clear-button");
clearButton.addEventListener("click", function () {
    form.reset();
    errorMessage.style.display = "none";
    cuentaInput.style.borderColor = "#ccc";
});
