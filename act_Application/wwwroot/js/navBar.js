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

modeSwitch.addEventListener("click", () => {
    body.classList.toggle("dark");

    if (body.classList.contains("dark")) {
        modeText.innerText = "Light mode";
    } else {
        modeText.innerText = "Dark mode";

    }
});