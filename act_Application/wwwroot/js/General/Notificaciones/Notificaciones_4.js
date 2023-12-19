//Validacion de Fechas.
document.addEventListener("DOMContentLoaded", function () {
    //Variables
    const fechaActual = new Date().toISOString().split("T")[0]; // Obtener la fecha actual en formato "yyyy-mm-dd"
    //Funciones de Validacion

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

    //Limites de Fechas.
    var fechaInicioPagoCuotaInput = document.getElementById('FechaInicioPagoCuotas');
    var fechaMinima = new Date();
    fechaMinima.setMonth(fechaMinima.getMonth() + 3);
    var fechaMinimaFormato = fechaMinima.toISOString().split('T')[0];
    fechaInicioPagoCuotaInput.setAttribute('min', fechaMinimaFormato);

    var fechaPagoTotalPrestamoInput = document.getElementById('FechaPagoTotalPrestamo');
    var fechaMinima = new Date();
    fechaMinima.setMonth(fechaMinima.getMonth() + 6);
    var fechaMinimaFormato = fechaMinima.toISOString().split('T')[0];
    fechaPagoTotalPrestamoInput.setAttribute('min', fechaMinimaFormato);

    var fechaInicioInput = document.getElementById('FechaInicio');
    var fechaMinima = new Date();
    var fechaMinimaFormato = fechaMinima.toISOString().split('T')[0];
    fechaInicioInput.setAttribute('min', fechaMinimaFormato);

    var fechaFinalizacionInput = document.getElementById('FechaFinalizacion');
    var fechaMinima = new Date();
    var fechaMinimaFormato = fechaMinima.toISOString().split('T')[0];
    fechaFinalizacionInput.setAttribute('min', fechaMinimaFormato);
});
