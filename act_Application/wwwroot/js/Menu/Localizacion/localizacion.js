async function getUserLocation() {
    try {
        const response = await fetch('https://ipinfo.io/json');
        if (response.ok) {
            const data = await response.json();
            const location = `${data.city}, ${data.region}, ${data.country}`;
            document.getElementById('userLocation').textContent = location;
        } else {
            console.error('Error al obtener la ubicación del usuario.');
        }
    } catch (error) {
        console.error('Error al obtener la ubicación del usuario.', error);
    }
}
