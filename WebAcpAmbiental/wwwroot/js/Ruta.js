
$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});



// CREACION DEL USUARIO

// Escuchar el evento de clic en el botón de "Save changes"
document.getElementById('saveChangesBtn').addEventListener('click', function () {

    const rutas = document.getElementById("rutas").value;
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
    fetch('/Ruta/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            descripcion: rutas,
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
                document.getElementById("rutas").value = '';
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


// ELIMINAR RUTA

$(document).ready(function () {
    // Botón eliminar ruta
    $(".btn-eliminar-ruta").on("click", function () {
        const idRuta = $(this).data("id");

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
                    url: `/Ruta/Eliminar/${idRuta}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "La ruta ha sido eliminada.", "success")
                                .then(() => location.reload()); // Recargar la página
                        } else {
                            Swal.fire("Error", response.message || "No se pudo eliminar la ruta.", "error");
                        }
                    },
                    error: function () {
                        Swal.fire("Error", "Ocurrió un problema al intentar eliminar la ruta.", "error");
                    }
                });
            }
        });
    });
});


// info ruta

function abrirModalRuta(button) {
    const id = button.getAttribute("data-id");

    fetch(`/Ruta/ObtenerRuta?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos de la ruta.");
            }
            return response.json();
        })
        .then(ruta => {
            // Verifica el contenido de los datos recibidos
            console.log(ruta);

            // Asigna los valores al modal

            document.getElementById("modalDescripcion").textContent = ruta.descripcion;
            document.getElementById("modalEstado").textContent = ruta.estado || "N/A";


            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalRutaDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información de la ruta.");
        });
}


// EDITAR RUTA

function editarRuta(button) {
    const idRuta = button.getAttribute("data-id");

    fetch(`/Ruta/ObtenerRuta?id=${idRuta}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Ruta no encontrado");
            }
            return response.json();
        })
        .then(ruta => {
            console.log("Datos de la ruta recibidos:", ruta);



            // Llenar los campos de texto
            document.getElementById("rutaEditar").value = ruta.descripcion;


            // Configurar el valor del select para Estado
            document.getElementById("estadoEditar").value = ruta.idEstado;

            // Guardar el ID en un atributo oculto
            document.getElementById("rutaEditar").dataset.id = ruta.idRuta;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarRuta"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos de la ruta:", error);
            alert("Error al cargar los datos de la ruta");
        });
}

document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const ruta = {
        idRuta: parseInt(document.getElementById("rutaEditar").dataset.id), // Captura el ID
        descripcion: document.getElementById("rutaEditar").value,
        idEstado: parseInt(document.getElementById("estadoEditar").value),

    };

    fetch("/Ruta/ActualizarRuta", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(ruta)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar la ruta");
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