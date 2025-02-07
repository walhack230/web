
$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});

document.getElementById('cbo-input').addEventListener('change', function () {
    const tipo = this.value;
    const nombres = document.getElementById('nombres');
    const apellidoPaterno = document.getElementById('apellidopaterno');
    const apellidoMaterno = document.getElementById('apellidomaterno');

    if (tipo === 'dni') {
        nombres.disabled = true;
        apellidoPaterno.disabled = true;
        apellidoMaterno.disabled = true;
    } else if (tipo === 'ce') {
        nombres.disabled = false;
        apellidoPaterno.disabled = false;
        apellidoMaterno.disabled = false;
    }
});

//BUSCAR DNI


document.addEventListener("DOMContentLoaded", function () {
    // Referencias a los elementos del DOM
    const dniInput = document.getElementById("documento");
    const btnBuscarDNI = document.getElementById("btnbuscar");
    const nombresInput = document.getElementById("nombres");
    const apellidoPaternoInput = document.getElementById("apellidopaterno");
    const apellidoMaternoInput = document.getElementById("apellidomaterno");

    // Evento para el botón de buscar DNI
    btnBuscarDNI.addEventListener("click", function () {
        const dni = dniInput.value.trim();

        // Validar el DNI
        if (dni === "" || dni.length !== 8 || isNaN(dni)) {
            alert("El DNI debe tener 8 dígitos numéricos.");
            return;
        }

        // Llamada a la API para buscar el DNI
        fetch(`/Usuario/BuscarPorDNI/${dni}`)
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

//CREACION DEL USUARIO

document.getElementById("saveChangesBtn").addEventListener("click", function () {
    const documento = document.getElementById("documento").value;
    const nombres = document.getElementById("nombres").value;
    const apellidopaterno = document.getElementById("apellidopaterno").value;
    const apellidomaterno = document.getElementById("apellidomaterno").value;
    const fechanacimiento = document.getElementById("fechanacimiento").value;
    const rol = document.getElementById("rol").value;
    const estado = document.getElementById("estado").value;
    const usuario = document.getElementById("usuario").value;
    const contraseña = document.getElementById("contraseña").value;

   

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
    fetch('/Usuario/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            nrodniUsuario: documento,
            nombreUsuario: nombres,
            apellidopUsuario: apellidopaterno,
            apellidomUsuario: apellidomaterno,
            fechNacimiento: fechanacimiento,
            userUsuario: usuario,
            passwordUsuario: contraseña,
            idRol: rol === "jefe" ? 1 : 2,
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


                //// Limpiar los campos del modal
                //$('#modal-default').modal('hide');
                //document.getElementById("nombres").value = '';
                //document.getElementById("apellidos").value = '';
                //document.getElementById("usuario").value = '';
                //document.getElementById("contraseña").value = '';
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





//Eliminar Usuario
$(document).ready(function () {
    // Botón eliminar usuario
    $(".btn-eliminar-usuario").on("click", function () {
        const idUsuario = $(this).data("id");

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
                    url: `/Usuario/Eliminar/${idUsuario}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "El usuario ha sido eliminado.", "success")
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

function abrirModalUsuario(button) {
    const id = button.getAttribute("data-id");

    fetch(`/Usuario/ObtenerUsuario?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos del usuario.");
            }
            return response.json();
        })
        .then(usuario => {
            // Verifica el contenido de los datos recibidos
            console.log(usuario);

            // Asigna los valores al modal
            document.getElementById("modalNombre").textContent =
                `${usuario.nombreUsuario} ${usuario.apellidopUsuario} ${usuario.apellidomUsuario}`;
            document.getElementById("modalUsuario").textContent = usuario.userUsuario;
            document.getElementById("modalEstado").textContent = usuario.estado || "N/A";
            document.getElementById("modalRol").textContent = usuario.rol || "N/A";

            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalUsuarioDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información del usuario.");
        });
}



function editarUsuario(button) {
    const idUsuario = button.getAttribute("data-id");

    fetch(`/Usuario/ObtenerUsuario?id=${idUsuario}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Usuario no encontrado");
            }
            return response.json();
        })
        .then(usuario => {
            console.log("Datos del usuario recibidos:", usuario);

           

            // Llenar los campos de texto
            document.getElementById("usuarioEditar").value = usuario.userUsuario;
            document.getElementById("dniEditar").value = usuario.nrodniUsuario;
            document.getElementById("nombresEditar").value = usuario.nombreUsuario;
            document.getElementById("apellidospEditar").value = usuario.apellidopUsuario;
            document.getElementById("apellidosmEditar").value = usuario.apellidomUsuario;
            document.getElementById("apellidosmEditar").value = usuario.apellidomUsuario;
           

            // Corregir el nombre de la propiedad de la contraseña
            document.getElementById("contraseñaEditar").value = usuario.passwordUsuario;

            // Configurar el valor del select para Rol
            document.getElementById("rolEditar").value = usuario.idRol;

            // Configurar el valor del select para Estado
            document.getElementById("estadoEditar").value = usuario.idEstado;

            // Guardar el ID en un atributo oculto
            document.getElementById("usuarioEditar").dataset.id = usuario.idUsuario;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarUsuario"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos del usuario:", error);
            alert("Error al cargar los datos del usuario");
        });
}


document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const usuario = {
        IdUsuario: parseInt(document.getElementById("usuarioEditar").dataset.id), // Captura el ID

        NrodniUsuario: document.getElementById("dniEditar").value,
        NombreUsuario: document.getElementById("nombresEditar").value,
        ApellidopUsuario: document.getElementById("apellidospEditar").value,
        ApellidomUsuario: document.getElementById("apellidosmEditar").value,
        UserUsuario: document.getElementById("usuarioEditar").value,
        PasswordUsuario: document.getElementById("contraseñaEditar").value,
        IdEstado: parseInt(document.getElementById("estadoEditar").value),
        IdRol: parseInt(document.getElementById("rolEditar").value),
    };

    fetch("/Usuario/ActualizarUsuario", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(usuario)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar el usuario");
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


document.getElementById("togglePassword").addEventListener("click", function () {
    const passwordField = document.getElementById("contraseñaEditar");
    const eyeIcon = document.getElementById("eyeIcon");

    // Cambiar el tipo del input entre "password" y "text"
    if (passwordField.type === "password") {
        passwordField.type = "text";
        eyeIcon.classList.remove("fa-eye");
        eyeIcon.classList.add("fa-eye-slash");
    } else {
        passwordField.type = "password";
        eyeIcon.classList.remove("fa-eye-slash");
        eyeIcon.classList.add("fa-eye");
    }
});











