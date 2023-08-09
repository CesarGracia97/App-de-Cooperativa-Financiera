document.addEventListener("DOMContentLoaded", function () {
    var cells = document.querySelectorAll(".aportacion-cell");

    cells.forEach(function (cell) {
        cell.addEventListener("mouseover", function () {
            var mensajeSinAportes = this.getAttribute("data-mensaje-sin-aportes");
            var mensajeConAportes = this.getAttribute("data-mensaje-con-aportes");
            var valor = parseFloat(this.innerText);

            if (valor === 0) {
                this.setAttribute("title", mensajeSinAportes);
            } else {
                this.setAttribute("title", mensajeConAportes);
            }
        });
    });
});