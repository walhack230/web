function editarProgramacion(button) {
    const idProgramacion = button.getAttribute("data-id");

    fetch(`/Programacion/ObtenerProgramacion?idProgramacion=${idProgramacion}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Programacion no encontrada");
            }
            return response.json();
        })
        .then(programacion => {
            console.log("Datos de la programacion recibida:", programacion);

            // Asegurar valores por defecto si son nulos
            document.getElementById("gestion").value = programacion.idGestion || "";
            document.getElementById("comentario").value = programacion.comentario || "";
            document.getElementById("proximaVisita").value = programacion.proximaVisita || "";

            // Guardar el ID en un atributo oculto
            document.getElementById("gestion").dataset.id = programacion.idProgramacion;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarProgramacion"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos de la programacion:", error);
            alert("Error al cargar los datos de la programacion");
        });
}



document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const idProgramacion = parseInt(document.getElementById("gestion").dataset.id);
    const proximaVisita = document.getElementById("proximaVisita").value || "No hay próxima visita";  // Si no hay fecha, se asigna un texto por defecto
    const comentario = document.getElementById("comentario").value || null;
    const idGestion = parseInt(document.getElementById("gestion").value) || null;

    // Validación solo de idProgramacion y idGestion
    if (!idProgramacion || !idGestion) {
        Swal.fire({
            icon: 'warning',
            title: 'Datos incompletos',
            text: 'Asegúrate de ingresar todos los datos correctamente',
            confirmButtonText: 'Aceptar'
        });
        return;
    }

    const programacion = {
        idProgramacion: idProgramacion,
        proximaVisita: proximaVisita === "No hay próxima visita" ? null : new Date(proximaVisita).toISOString(), // Si se puso el texto, lo enviamos como null
        comentario: comentario,
        idGestion: idGestion
    };

    fetch("/Programacion/ActualizarProgramacion", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(programacion)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar la programación");
            }
            return response.json();
        })
        .then(data => {
            Swal.fire({
                icon: 'success',
                title: '¡Éxito!',
                text: data.message,
                confirmButtonText: 'Aceptar',
                timer: 3000
            }).then(() => {
                location.reload();
            });
        })
        .catch(error => {
            console.error(error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo actualizar la programación',
                confirmButtonText: 'Cerrar'
            });
        });
});


$(document).ready(function () {
    // Capturar el evento de apertura del modal
    $("#programacionModal").on("show.bs.modal", function (event) {
        var button = $(event.relatedTarget); // Botón que activó el modal
        var idUsuario = button.data("idusuario"); // Obtener el idUsuario del atributo data-idusuario

        console.log("ID Usuario capturado:", idUsuario); // Verifica en la consola si se captura correctamente

        cargarProgramacion(idUsuario); // Llamar a la función con el idUsuario dinámico
    });
});

function cargarProgramacion(idUsuario) {
    $.ajax({
        url: `https://localhost:7184/GetProgramaciones?idUsuario=${idUsuario}`, // URL completa
        type: "GET",
        dataType: "json",
        success: function (data) {
            let tbody = $("#programacionTableBody");
            tbody.empty(); // Limpiar la tabla antes de agregar nuevos datos

            if (data.length === 0) {
                tbody.append("<tr><td colspan='3' class='text-center'>No hay datos disponibles</td></tr>");
            } else {
                data.forEach((item) => {
                    let fila = `
                        <tr>
                            <td>${item.dia}</td>
                            <td>${item.distrito}</td>
                            <td>${item.cantidad}</td>
                        </tr>`;
                    tbody.append(fila);
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al cargar la programación:", error);
            let tbody = $("#programacionTableBody");
            tbody.empty();
            tbody.append("<tr><td colspan='3' class='text-center text-danger'>Error al cargar los datos</td></tr>");
        }
    });
}


$(document).ready(function () {
    // Cambiar el estado de las filas con información a "Trabajado"
    $("tr.con-informacion").each(function () {
        $(this).find("td:nth-child(10)").text("Trabajado");  // Cambiar la columna de Estado a "Trabajado"
    });
});


$(function () {
    $("#example2").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');

});
