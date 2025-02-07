

$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});

//registrar canal
document.getElementById('saveChangesBtn').addEventListener('click', function () {

    const canal = document.getElementById("canal").value;
    const estado = document.getElementById("estado").value;

    // Verificar si el estado es "inactivo"
    if (estado === "inactivo") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Una ruta no puede estar con el estado inactivo.',
        });
        return;
    }

    // Hacer el registro en la base de datos
    fetch('/CanalVenta/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            descripcion: canal,
            idEstado: estado === "activo" ? 1 : 2
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Registro exitoso',
                    text: 'El registro se realizó correctamente.',
                    showConfirmButton: false,  // Elimina el botón de confirmación si no es necesario
                    timer: 3000,  // Duración en milisegundos (3 segundos)
                }).then(() => {
                    location.reload();  // Recarga la página una vez que el SweetAlert haya terminado
                });


                // Limpiar los campos del modal
                $('#modal-default').modal('hide');
                document.getElementById("canal").value = '';
                document.getElementById("estado").value = '';

            } else {
                // Si hubo un error en la respuesta del servidor
                Swal.fire({
                    icon: 'error',
                    title: 'Error al registrar',
                    text: 'Hubo un problema al registrar la ruta.',
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

//eliminar canal
$(document).ready(function () {
    // Botón eliminar ruta
    $(".btn-eliminar-canal").on("click", function () {
        const idCanal = $(this).data("id");

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
                    url: `/CanalVenta/Eliminar/${idCanal}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "El canal ha sido eliminado.", "success")
                                .then(() => location.reload()); // Recargar la página
                        } else {
                            Swal.fire("Error", response.message || "No se pudo eliminar el canal.", "error");
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


//info canal


function abrirModalCanal(button) {
    const id = button.getAttribute("data-id");

    fetch(`/CanalVenta/ObtenerCanalVenta?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos del canal.");
            }
            return response.json();
        })
        .then(canal => {
            // Verifica el contenido de los datos recibidos
            console.log(canal);

            // Asigna los valores al modal

            document.getElementById("modalDescripcion").textContent = canal.descripcion;
            document.getElementById("modalEstado").textContent = canal.estado || "N/A";


            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalCanalDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información del canal.");
        });
}

// EDITAR CANAL

function editarCanal(button) {
    const idCanal = button.getAttribute("data-id");

    fetch(`/CanalVenta/ObtenerCanalVenta?id=${idCanal}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Canal no encontrado");
            }
            return response.json();
        })
        .then(canal => {
            console.log("Datos del canal recibidos:", canal);



            // Llenar los campos de texto
            document.getElementById("canalEditar").value = canal.descripcion;


            // Configurar el valor del select para Estado
            document.getElementById("estadoEditar").value = canal.idEstado;

            // Guardar el ID en un atributo oculto
            document.getElementById("canalEditar").dataset.id = canal.idCanal;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarCanal"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos del canal:", error);
            alert("Error al cargar los datos del canal");
        });
}



document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const canal = {
        idCanal: parseInt(document.getElementById("canalEditar").dataset.id), // Captura el ID
        descripcion: document.getElementById("canalEditar").value,
        idEstado: parseInt(document.getElementById("estadoEditar").value),

    };

    fetch("/CanalVenta/ActualizarCanalVenta", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(canal)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar al canal");
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
            alert("Error al guardar los cambios");
        });
});

