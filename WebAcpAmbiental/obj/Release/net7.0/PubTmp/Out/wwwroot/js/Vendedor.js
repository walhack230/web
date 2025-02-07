
$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});

// Escuchar el evento de clic en el botón de "Save changes"
document.getElementById('saveChangesBtn').addEventListener('click', function () {
    // Mostrar el SweetAlert de éxito
    Swal.fire({
        icon: 'success',
        title: '¡Registro de vendedor exitosa!',
        showConfirmButton: false,
        timer: 1500 // El mensaje desaparece después de 1.5 segundos
    }).then(() => {
        // Puedes redirigir o hacer alguna acción después de mostrar la alerta
        // Por ejemplo, cerrar el modal
        $('#modal-default').modal('hide');
    });
});

//para el registro

document.addEventListener("DOMContentLoaded", function () {
    // Referencias a los elementos del DOM
    const dniInput = document.getElementById("dni");
    const btnBuscarDNI = document.getElementById("btnBuscarDNI");
    const nombresInput = document.getElementById("nombres");
    const apellidoPaternoInput = document.getElementById("apellidoPaterno");
    const apellidoMaternoInput = document.getElementById("apellidoMaterno");

    // Evento para el botón de buscar DNI
    btnBuscarDNI.addEventListener("click", function () {
        const dni = dniInput.value.trim();

        // Validar el DNI
        if (dni === "" || dni.length !== 8 || isNaN(dni)) {
            alert("El DNI debe tener 8 dígitos numéricos.");
            return;
        }

        // Llamada a la API para buscar el DNI
        fetch(`/Vendedor/BuscarPorDNI/${dni}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("No se encontró información para el DNI proporcionado.");
                }
                return response.json();
            })
            .then(data => {
                // Rellenar los campos del formulario con los datos obtenidos
                nombresInput.value = data.nombres || "";
                apellidoPaternoInput.value = data.apellidoPaterno || "";
                apellidoMaternoInput.value = data.apellidoMaterno || "";
            })
            .catch(error => {
                // Mostrar un mensaje de error si no se encuentra el DNI
                alert(error.message);
            });
    });
});

//para el editar

document.addEventListener("DOMContentLoaded", function () {
    // Referencias a los elementos del DOM para editar
    const dniInputEditar = document.getElementById("dniEditar");
    const btnBuscarDNIEditar = document.getElementById("btnBuscarDNIEDITAR");
    const nombresInputEditar = document.getElementById("nombresEditar");
    const apellidoPaternoInputEditar = document.getElementById("apellidoPaternoEditar");
    const apellidoMaternoInputEditar = document.getElementById("apellidoMaternoEditar");

    // Evento para el botón de buscar DNI en el formulario de edición
    btnBuscarDNIEditar.addEventListener("click", function () {
        const dni = dniInputEditar.value.trim();

        // Validar el DNI
        if (dni === "" || dni.length !== 8 || isNaN(dni)) {
            alert("El DNI debe tener 8 dígitos numéricos.");
            return;
        }

        // Llamada a la API para buscar el DNI
        fetch(`/Vendedor/BuscarPorDNI/${dni}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error("No se encontró información para el DNI proporcionado.");
                }
                return response.json();
            })
            .then(data => {
                // Rellenar los campos del formulario de edición con los datos obtenidos
                nombresInputEditar.value = data.nombres || "";
                apellidoPaternoInputEditar.value = data.apellidoPaterno || "";
                apellidoMaternoInputEditar.value = data.apellidoMaterno || "";
            })
            .catch(error => {
                // Mostrar un mensaje de error si no se encuentra el DNI
                alert(error.message);
            });
    });
});



document.getElementById("saveChangesBtn").addEventListener("click", function () {
    const dni = document.getElementById("dni").value;
    const nombres = document.getElementById("nombres").value;
    const apellidoPaterno = document.getElementById("apellidoPaterno").value;
    const apellidoMaterno = document.getElementById("apellidoMaterno").value;
    const fechaNacimiento = document.getElementById("fechaNacimiento").value;
    const estado = document.getElementById("estado").value;
   

    // Validar si los campos obligatorios están vacíos
    if (!nombres || !apellidoPaterno || !apellidoMaterno || !dni || !fechaNacimiento) {
        alert("Por favor, complete todos los campos obligatorios.");
        return;
    }

    // Verificar si el estado es "inactivo"
    if (estado === "inactivo") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Un usuario no puede estar con el estado inactivo.',
        });
        return;
    }

    // Hacer el registro en la base de datos
    fetch('/Vendedor/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            dniVendedor: dni,
            nombreVendedor: nombres,
            apellidopVendedor: apellidoPaterno,
            apellidomVendedor: apellidoMaterno,
            fechnacVendedor: fechaNacimiento,
            IdEstado: estado === "activo" ? 1 : 2
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
                document.getElementById("dni").value = '';
                document.getElementById("nombres").value = '';
                document.getElementById("apellidoPaterno").value = '';
                document.getElementById("apellidoMaterno").value = '';
                document.getElementById("fechaNacimiento").value = '';
                
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


//Eliminar Vendedor
$(document).ready(function () {
    // Botón eliminar usuario
    $(".btn-eliminar-vendedor").on("click", function () {
        const idVendedor = $(this).data("id");

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
                    url: `/Vendedor/Eliminar/${idVendedor}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "El vendedor ha sido eliminado.", "success")
                                .then(() => location.reload()); // Recargar la página
                        } else {
                            Swal.fire("Error", response.message || "No se pudo eliminar el vendedor.", "error");
                        }
                    },
                    error: function () {
                        Swal.fire("Error", "Ocurrió un problema al intentar eliminar al vendedor.", "error");
                    }
                });
            }
        });
    });
});

function abrirModalVendedor(button) {
    const id = button.getAttribute("data-id");

    fetch(`/Vendedor/ObtenerVendedor?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos del vendedor.");
            }
            return response.json();
        })
        .then(vendedor => {
            // Verifica el contenido de los datos recibidos
            console.log(vendedor);

            // Asigna los valores al modal
            document.getElementById("modalDni").textContent = vendedor.dniVendedor;
            document.getElementById("modalNombres").textContent =
                `${vendedor.nombreVendedor} ${vendedor.apellidopVendedor} ${vendedor.apellidomVendedor}`;
            document.getElementById("modalFecha").textContent = vendedor.fechnacVendedor || "N/A";
            document.getElementById("modalEstado").textContent = vendedor.estado || "N/A";
          

            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalVendedorDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información del vendedor.");
        });
}



function editarVendedor(button) {
    const idVendedor = button.getAttribute("data-id");

    fetch(`/Vendedor/ObtenerVendedor?id=${idVendedor}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Vendedor no encontrado");
            }
            return response.json();
        })
        .then(vendedor => {
            console.log("Datos del vendedor recibidos:", vendedor);

         

            // Llenar los campos de texto
            document.getElementById("dniEditar").value = vendedor.dniVendedor;
            document.getElementById("nombresEditar").value = vendedor.nombreVendedor;
            document.getElementById("apellidoPaternoEditar").value = vendedor.apellidopVendedor;
            document.getElementById("apellidoMaternoEditar").value = vendedor.apellidomVendedor;
            // Convertir la fecha al formato YYYY-MM-DD
            const fechaNacimiento = new Date(vendedor.fechnacVendedor).toISOString().split('T')[0];
            document.getElementById("fechaNacimientoEditar").value = fechaNacimiento;

            // Configurar el valor del select para Estado
            document.getElementById("estadoEditar").value = vendedor.idEstado;

            // Guardar el ID en un atributo oculto
            document.getElementById("dniEditar").dataset.id = vendedor.idVendedor;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarVendedor"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos del vendedor:", error);
            alert("Error al cargar los datos del vendedor");
        });
}




document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const vendedor = {
        IdVendedor: parseInt(document.getElementById("dniEditar").dataset.id), // Captura el ID
        DniVendedor: document.getElementById("dniEditar").value,
        NombreVendedor: document.getElementById("nombresEditar").value,
        ApellidopVendedor: document.getElementById("apellidoPaternoEditar").value,
        ApellidomVendedor: document.getElementById("apellidoMaternoEditar").value,
        FechnacVendedor: document.getElementById("fechaNacimientoEditar").value,
        IdEstado: parseInt(document.getElementById("estadoEditar").value),
        
    };

    fetch("/Vendedor/ActualizarVendedor", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(vendedor)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar al vendedor");
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
