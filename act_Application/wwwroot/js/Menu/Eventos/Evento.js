document.addEventListener("DOMContentLoaded", function () {
    // Oculta la ventana modal por defecto
    $("#eventos-modal").hide();

    // Cuando se hace clic en el botón con la clase 'btn-participante'
    $("#btn-participante").click(function () {
        // Muestra la ventana modal
        $("#eventos-modal").fadeIn();
    });

    // Cuando se hace clic en el enlace con la clase 'close-modal'
    $(".close-modal").click(function () {
        // Oculta la ventana modal
        $("#eventos-modal").fadeOut();
    });
});
