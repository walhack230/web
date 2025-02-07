
$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});

// Escuchar el evento de clic en el botón de "Save changes"
document.getElementById('saveChangesBtn').addEventListener('click', function () {

    const zonas = document.getElementById("zonas").value;
    const estado = document.getElementById("estado").value;
   
    // Verificar si el estado es "inactivo"
    if (estado === "inactivo") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Una zona no puede estar con el estado inactivo.',
        });
        return;
    }

    // Hacer el registro en la base de datos
    fetch('/Zona/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            descripcion: zonas,
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
                document.getElementById("zonas").value = '';
                document.getElementById("estado").value = '';
               
            } else {
                // Si hubo un error en la respuesta del servidor
                Swal.fire({
                    icon: 'error',
                    title: 'Error al registrar',
                    text: 'Hubo un problema al registrar el usuario.',
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


//Eliminar Zona
$(document).ready(function () {
    // Botón eliminar zona
    $(".btn-eliminar-zona").on("click", function () {
        const idZona = $(this).data("id");

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
                // Llamada AJAX para eliminar el usuario
                $.ajax({
                    url: `/Zona/Eliminar/${idZona}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "La zona ha sido eliminada.", "success")
                                .then(() => location.reload()); // Recargar la página
                        } else {
                            Swal.fire("Error", response.message || "No se pudo eliminar el usuario.", "error");
                        }
                    },
                    error: function () {
                        Swal.fire("Error", "Ocurrió un problema al intentar eliminar el usuario.", "error");
                    }
                });
            }
        });
    });
});



//info zona

function abrirModalZona(button) {
    const id = button.getAttribute("data-id");

    fetch(`/Zona/ObtenerZona?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos de la zona.");
            }
            return response.json();
        })
        .then(zona => {
            // Verifica el contenido de los datos recibidos
            console.log(zona);

            // Asigna los valores al modal

            document.getElementById("modalDescripcion").textContent = zona.descripcion;
            document.getElementById("modalEstado").textContent = zona.estado || "N/A";
           

            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalZonaDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información de la zona.");
        });
}


//EDITAR


function editarZona(button) {
    const idZona = button.getAttribute("data-id");

    fetch(`/Zona/ObtenerZona?id=${idZona}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Zona no encontrado");
            }
            return response.json();
        })
        .then(zona => {
            console.log("Datos de la zona recibidos:", zona);

           

            // Llenar los campos de texto
            document.getElementById("zonaEditar").value = zona.descripcion;
           

            // Configurar el valor del select para Estado
            document.getElementById("estadoEditar").value = zona.idEstado;

            // Guardar el ID en un atributo oculto
            document.getElementById("zonaEditar").dataset.id = zona.idZona;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarZona"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos de la zona:", error);
            alert("Error al cargar los datos de la zona");
        });
}


document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const zona = {
        idZona: parseInt(document.getElementById("zonaEditar").dataset.id), // Captura el ID
        descripcion: document.getElementById("zonaEditar").value,
        idEstado: parseInt(document.getElementById("estadoEditar").value),
        
    };

    fetch("/Zona/ActualizarZona", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(zona)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar la zona");
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





