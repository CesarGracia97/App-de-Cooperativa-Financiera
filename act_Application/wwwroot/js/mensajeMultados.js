document.addEventListener("DOMContentLoaded", function () {
    var cells = document.querySelectorAll(".multa-cell");

    cells.forEach(function (cell) {
        cell.addEventListener("mouseover", function () {
            var mensajeSinMultas = this.getAttribute("data-mensaje-sin-multas");
            var mensajeConMultas = this.getAttribute("data-mensaje-con-multas");
            var valor = parseFloat(this.innerText);

            if (valor === 0) {
                this.setAttribute("title", mensajeSinMultas);
            } else {
                this.setAttribute("title", mensajeConMultas);
            }
        });
    });
});