const body = document.querySelector('body'),
    sidebar = body.querySelector('nav'),
    toggle = body.querySelector(".toggle"),
    searchBtn = body.querySelector(".search-box"),
    modeSwitch = body.querySelector(".toggle-switch"),
    modeText = body.querySelector(".mode-text");

// Recuperar el estado del Navbar al cargar la página
document.addEventListener("DOMContentLoaded", () => {
    const navbarState = localStorage.getItem("navbarState");
    if (navbarState === "expanded") {
        sidebar.classList.remove("close");
    } else {
        sidebar.classList.add("close");
    }
});

toggle.addEventListener("click", () => {
    sidebar.classList.toggle("close");
    // Al hacer clic en el toggle, guardar el estado en el almacenamiento local
    const navbarState = sidebar.classList.contains("close") ? "collapsed" : "expanded";
    localStorage.setItem("navbarState", navbarState);
})

searchBtn.addEventListener("click", () => {
    sidebar.classList.remove("close");
})

// Función para establecer el modo (oscuro o luz)
function setMode(mode) {
    if (mode === "dark") {
        body.classList.add("dark");
        modeText.innerText = "Light mode";
    } else {
        body.classList.remove("dark");
        modeText.innerText = "Dark mode";
    }
}

// Recuperar el estado del modo al cargar la página
document.addEventListener("DOMContentLoaded", () => {
    const storedMode = localStorage.getItem("mode");
    if (storedMode) {
        setMode(storedMode);
    }
});

modeSwitch.addEventListener("click", () => {
    body.classList.toggle("dark");
    const currentMode = body.classList.contains("dark") ? "dark" : "light";
    setMode(currentMode);
    // Guardar el estado del modo en el almacenamiento local
    localStorage.setItem("mode", currentMode);
});