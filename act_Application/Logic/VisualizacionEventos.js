document.addEventListener("DOMContentLoaded", function () {
    var userIdClaim = document.querySelector('[data-claim-type="Id"]');
    var userNameClaim = document.querySelector('[data-claim-type="Name"]');

    var userId = userIdClaim ? userIdClaim.textContent.trim() : '';
    var userName = userNameClaim ? userNameClaim.textContent.trim() : '';
    var eventos = document.querySelectorAll('.eventos-modal');

    eventos.forEach(function (evento) {
        var participantesId = evento.querySelector('.modal-evento-titulo').textContent.split('#')[1].trim();
        var participantesNombre = evento.querySelector('.modal-parrafo:nth-child(2)').textContent.split(':')[1].trim();

        // Verificar si el usuario está participando en este evento
        if (!participantesId.includes(userId) || !participantesNombre.includes(userName)) {
            evento.style.display = 'none'; // Ocultar el evento si el usuario no está participando
        }
    });

});