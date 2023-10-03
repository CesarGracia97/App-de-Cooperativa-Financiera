document.addEventListener("DOMContentLoaded", function () {
    // Oculta todas las ventanas modales por defecto
    $(".eventos-modal").hide();

    // Cuando se hace clic en cualquier botón con la clase 'btn-participante'
    $(".btn-participante").click(function () {
        // Obtén el identificador único del evento desde el atributo data
        var eventId = $(this).data("event-id");

        // Muestra la ventana modal correspondiente
        $("#eventos-modal-" + eventId).fadeIn();
    });

    // Cuando se hace clic en cualquier enlace con la clase 'close-modal'
    $(".close-modal").click(function () {
        // Oculta la ventana modal correspondiente
        var eventId = $(this).data("event-id");
        $("#eventos-modal-" + eventId).fadeOut();
    });
});
