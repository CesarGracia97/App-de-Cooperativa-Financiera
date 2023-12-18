document.addEventListener("DOMContentLoaded", function () {
    // Obtener referencias a los elementos
    var selectElement = document.querySelector('.select-acep-dene');
    var sContainer = document.querySelector('.S-container-modal-btn');
    var nContainer = document.querySelector('.N-container-modal-btn');

    // Ocultar los contenedores por defecto
    sContainer.style.display = 'none';
    nContainer.style.display = 'none';

    // Manejar el evento de cambio en el select
    selectElement.addEventListener('change', function () {
        // Ocultar todos los contenedores
        sContainer.style.display = 'none';
        nContainer.style.display = 'none';

        // Mostrar el contenedor correspondiente según la opción seleccionada
        if (selectElement.value === 'SI') {
            sContainer.style.display = 'block';
        } else if (selectElement.value === 'NO') {
            nContainer.style.display = 'block';
        }
    });
});