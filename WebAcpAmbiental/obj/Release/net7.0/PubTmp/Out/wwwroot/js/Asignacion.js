
$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});

$(function () {
    $("#example2").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');

});



    // Función para llenar los select con datos de la base de datos
    $(document).ready(function() {
        // Llenar las rutas
        $.ajax({
            url: '/Asignacion/GetRutas',
            type: 'GET',
            success: function (data) {
                var rutasSelect = $('#ruta');
                rutasSelect.empty();  // Limpiar las opciones previas
                rutasSelect.append('<option value="">Seleccione una Ruta</option>'); // Agregar opción por defecto
                data.forEach(function (ruta) {
                    rutasSelect.append('<option value="' + ruta.idRuta + '">' + ruta.descripcion + '</option>');
                });
            }
        });

    // Llenar las zonas
    $.ajax({
        url: '/Asignacion/GetZonas',
    type: 'GET',
    success: function(data) {
                var zonasSelect = $('#zona');
    zonasSelect.empty();  // Limpiar las opciones previas
    zonasSelect.append('<option value="">Seleccione una Zona</option>'); // Agregar opción por defecto
    data.forEach(function(zona) {
        zonasSelect.append('<option value="' + zona.idZona + '">' + zona.descripcion + '</option>');
                });
            }
        });

    // Llenar los canales
    $.ajax({
        url: '/Asignacion/GetCanalesVenta',
    type: 'GET',
    success: function(data) {
                var canalesSelect = $('#canal');
    canalesSelect.empty();  // Limpiar las opciones previas
    canalesSelect.append('<option value="">Seleccione un Canal</option>'); // Agregar opción por defecto
    data.forEach(function(canal) {
        canalesSelect.append('<option value="' + canal.idCanal + '">' + canal.descripcion + '</option>');
                });
            }
        });

    // Llenar los usuarios
    $.ajax({
        url: '/Asignacion/GetUsuario',
    type: 'GET',
    success: function(data) {
                var usuarioSelect = $('#usuario');
    usuarioSelect.empty();  // Limpiar las opciones previas
    usuarioSelect.append('<option value="">Seleccione un Usuario</option>'); // Agregar opción por defecto
    data.forEach(function(usuario) {
        usuarioSelect.append('<option value="' + usuario.idUsuario + '">' + usuario.nombreCompleto + '</option>');
                });
            }
        });
    });

document.getElementById('saveChangesBtn').addEventListener('click', function () {
    const canal = document.getElementById("canal").value;
    const ruta = document.getElementById("ruta").value;
    const zona = document.getElementById("zona").value;
    const usuario = document.getElementById("usuario").value;

    if (!canal || !ruta || !zona || !usuario) {
        Swal.fire({
            icon: 'warning',
            title: 'Campos incompletos',
            text: 'Por favor, completa todos los campos antes de continuar.',
        });
        return;
    }

    // Continúa con el fetch si todos los campos son válidos
    fetch('/Asignacion/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            idCanal: canal,
            idRuta: ruta,
            idZona: zona,
            idUsuario: usuario
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Registro exitoso',
                    text: 'El registro se realizó correctamente.',
                    showConfirmButton: false,
                    timer: 3000,
                }).then(() => {
                    location.reload();
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al registrar',
                    text: data.message || 'Hubo un problema al registrar la asignación.',
                });
            }
        })
        .catch(error => {
            console.error("Error:", error);
            Swal.fire({
                icon: 'error',
                title: 'Error de conexión',
                text: 'No se pudo conectar con el servidor. Inténtelo nuevamente.',
            });
        });
});




$(document).ready(function () {
    // Botón eliminar ruta
    $(".btn-eliminar-asignacion").on("click", function () {
        const idAsignacion = $(this).data("id");

        // Mostrar SweetAlert de confirmación
        Swal.fire({
            title: "¿Estás seguro?",
            text: "¡No podrás revertir esta acción!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d33",
            cancelButtonColor: "#3085d6",
            confirmButtonText: "Sí, eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                // Llamada AJAX para eliminar la ruta
                $.ajax({
                    url: `/Asignacion/Eliminar/${idAsignacion}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "La asignacion ha sido eliminada.", "success")
                                .then(() => location.reload()); // Recargar la página
                        } else {
                            Swal.fire("Error", response.message || "No se pudo eliminar la asignacion.", "error");
                        }
                    },
                    error: function () {
                        Swal.fire("Error", "Ocurrió un problema al intentar eliminar el canal.", "error");
                    }
                });
            }
        });
    });
});

function abrirModalAsignacion(button) {
    const id = button.getAttribute("data-id");

    fetch(`/Asignacion/ObtenerAsignacion?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos de la asignacion.");
            }
            return response.json();
        })
        .then(asignacion => {
            // Verifica el contenido de los datos recibidos
            console.log(asignacion);

            // Asigna los valores al modal

            document.getElementById("zonaDescripcion").textContent = asignacion.zona || "N/A";
            document.getElementById("rutaDescripcion").textContent = asignacion.ruta || "N/A";
            document.getElementById("canalDescripcion").textContent = asignacion.canal || "N/A";
            document.getElementById("usuarioDescripcion").textContent = asignacion.usuario || "N/A";


            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalAsignacionDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información de la Asignacion.");
        });
}

function editarAsignacion(button) {
    const idAsignacion = button.getAttribute("data-id");

    fetch(`/Asignacion/ObtenerAsignacion?id=${idAsignacion}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Asignacion no encontrada");
            }
            return response.json();
        })
        .then(asignacion => {
            console.log("Datos del usuario recibidos:", asignacion);

            // Llena las opciones de los select dinámicamente si aún no están cargadas
            fetch(`/Asignacion/GetZonas`)
                .then(response => response.json())
                .then(zonas => {
                    const zonaSelect = document.getElementById("zonaEditar");
                    zonaSelect.innerHTML = ''; // Limpia opciones anteriores
                    zonas.forEach(zona => {
                        const option = document.createElement("option");
                        option.value = zona.idZona;
                        option.textContent = zona.descripcion;
                        zonaSelect.appendChild(option);
                    });
                    zonaSelect.value = asignacion.idZona; // Selecciona el valor correcto
                });

            fetch(`/Asignacion/GetCanalesVenta`)
                .then(response => response.json())
                .then(canales => {
                    const canalSelect = document.getElementById("canalEditar");
                    canalSelect.innerHTML = ''; // Limpia opciones anteriores
                    canales.forEach(canal => {
                        const option = document.createElement("option");
                        option.value = canal.idCanal;
                        option.textContent = canal.descripcion;
                        canalSelect.appendChild(option);
                    });
                    canalSelect.value = asignacion.idCanal; // Selecciona el valor correcto
                });

            fetch(`/Asignacion/GetRutas`)
                .then(response => response.json())
                .then(rutas => {
                    const rutaSelect = document.getElementById("rutaEditar");
                    rutaSelect.innerHTML = ''; // Limpia opciones anteriores
                    rutas.forEach(ruta => {
                        const option = document.createElement("option");
                        option.value = ruta.idRuta;
                        option.textContent = ruta.descripcion;
                        rutaSelect.appendChild(option);
                    });
                    rutaSelect.value = asignacion.idRuta; // Selecciona el valor correcto
                });

            fetch(`/Asignacion/GetUsuario`)
                .then(response => response.json())
                .then(usuarios => {
                    const usuarioSelect = document.getElementById("usuarioEditar");
                    usuarioSelect.innerHTML = ''; // Limpia opciones anteriores
                    usuarios.forEach(usuario => {
                        const option = document.createElement("option");
                        option.value = usuario.idUsuario;
                        option.textContent = usuario.nombreCompleto;
                        usuarioSelect.appendChild(option);
                    });
                    usuarioSelect.value = asignacion.idUsuario; // Selecciona el valor correcto
                });

            // Guardar el ID en un atributo oculto
            document.getElementById("usuarioEditar").dataset.id = asignacion.idAsignacion;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarAsignacion"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos del usuario:", error);
            alert("Error al cargar los datos del usuario");
        });
}



document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const asignacion = {
        IdAsignacion: parseInt(document.getElementById("usuarioEditar").dataset.id), // Captura el ID de la asignación
        IdCanal: parseInt(document.getElementById("canalEditar").value), // Obtén el canal seleccionado
        IdZona: parseInt(document.getElementById("zonaEditar").value), // Obtén la zona seleccionada
        IdRuta: parseInt(document.getElementById("rutaEditar").value), // Obtén la ruta seleccionada
        IdUsuario: parseInt(document.getElementById("usuarioEditar").value) // Obtén el usuario seleccionado
    };

    fetch("/Asignacion/ActualizarAsignacion", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(asignacion)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar la asignación");
            }
            return response.json();
        })
        .then(data => {
            Swal.fire({
                icon: 'success',
                title: '¡Éxito!',
                text: data.message,
                confirmButtonText: 'Aceptar',
                timer: 3000 // Cierra automáticamente después de 3 segundos
            }).then(() => {
                location.reload(); // Recargar la página o actualizar la tabla
            });
        })
        .catch(error => {
            console.error(error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al guardar los cambios.',
                confirmButtonText: 'Aceptar'
            });
        });
});

