
function showLoaderAndRedirect(event) {
    event.preventDefault(); // Previene el envío inmediato del formulario
    const loaderOverlay = document.getElementById('loader-overlay');
    loaderOverlay.style.visibility = 'visible'; // Muestra el loader

    // Esperar el tiempo del loader antes de enviar el formulario
    setTimeout(() => {
        // Ahora se envía el formulario después de mostrar el loader
        event.target.submit();
    }, 3000); // Ajusta el tiempo del loader (en milisegundos)
}

