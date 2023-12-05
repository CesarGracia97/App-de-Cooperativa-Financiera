// Guarda esto en un archivo JS (por ejemplo, script.js)
document.addEventListener('DOMContentLoaded', function () {
    // Obtén la referencia al elemento select
    var selectElement = document.querySelector('.select-opcion');

    // Obtén las referencias a los elementos div que quieres mostrar/ocultar
    var c_apor = document.querySelector('.c_apor');
    var c_pres = document.querySelector('.c_pres');
    var c_pcuo = document.querySelector('.c_pcuo');
    var c_pmul = document.querySelector('.c_pmul');

    // Función para mostrar u ocultar los div según la opción seleccionada
    function mostrarOcultarDivs() {
        // Oculta todos los divs
        c_apor.style.display = 'none';
        c_pres.style.display = 'none';
        c_pcuo.style.display = 'none';
        c_pmul.style.display = 'none';

        // Muestra el div correspondiente a la opción seleccionada
        switch (selectElement.value) {
            case 'Aporte':
                c_apor.style.display = 'block';
                break;
            case 'Prestamo':
                c_pres.style.display = 'block';
                break;
            case 'PCuota':
                c_pcuo.style.display = 'block';
                break;
            case 'PMulta':
                c_pmul.style.display = 'block';
                break;
            default:
                break;
        }
    }

    // Agrega un evento de cambio al elemento select
    selectElement.addEventListener('change', mostrarOcultarDivs);

    // Llama a la función inicialmente para establecer el estado inicial
    mostrarOcultarDivs();
});
