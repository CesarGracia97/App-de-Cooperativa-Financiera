
var inactivityTimeout = 900000; //900000 15 minutos; 60000 es un minuto en milisegundos

var inactivityTimer;

function resetInactivityTimer() {
    clearTimeout(inactivityTimer);
    inactivityTimer = setTimeout(logout, inactivityTimeout);
}

function logout() {
    // Simular un clic en el botón de cierre de sesión
    document.getElementById("btn-CerrarSesion").click();
}

$(document).on('mousemove keydown', function () {
    resetInactivityTimer();
});

// Iniciar el temporizador de inactividad al cargar la página
$(document).ready(function () {
    resetInactivityTimer();
});
