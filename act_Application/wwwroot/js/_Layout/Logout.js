// DeslogeoAutomático después de 1 minuto de inactividad
var inactivityTimeout = 60000; // 1 minuto en milisegundos

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
