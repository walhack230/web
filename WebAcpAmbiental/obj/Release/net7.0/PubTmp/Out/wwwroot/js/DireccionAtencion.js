$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});


//DEPARTAMENTOS PROVINCIAS DISTRITOS
$(document).ready(function () {
    // Cargar departamentos al cargar la página
    $.getJSON('/DireccionAtencion/GetDepartamentos', function (data) {
        let departamentoSelect = $('#departamentoDireccion');
        data.forEach(function (item) {
            departamentoSelect.append(
                $('<option>', { value: item.idDepartamento, text: item.nombreDepartamento })
            );
        });
        departamentoSelect.selectpicker('refresh');
    });

    // Cargar provincias según el departamento seleccionado
    $('#departamentoDireccion').change(function () {
        let idDepartamento = $(this).val();
        let provinciaSelect = $('#provinciaDireccion');
        provinciaSelect.empty().append('<option value="">Seleccione una provincia</option>');

        if (idDepartamento) {
            $.getJSON(`/DireccionAtencion/GetProvincias?idDepartamento=${idDepartamento}`, function (data) {
                data.forEach(function (item) {
                    provinciaSelect.append(
                        $('<option>', { value: item.idProvincia, text: item.nombreProvincia })
                    );
                });
                provinciaSelect.selectpicker('refresh');
            });
        }
        $('#distritoDireccion').empty().append('<option value="">Seleccione un distrito</option>').selectpicker('refresh');
    });

    // Cargar distritos según la provincia seleccionada
    $('#provinciaDireccion').change(function () {
        let idProvincia = $(this).val();
        let distritoSelect = $('#distritoDireccion');
        distritoSelect.empty().append('<option value="">Seleccione un distrito</option>');

        if (idProvincia) {
            $.getJSON(`/DireccionAtencion/GetDistritos?idProvincia=${idProvincia}`, function (data) {
                data.forEach(function (item) {
                    distritoSelect.append(
                        $('<option>', { value: item.idDistrito, text: item.nombreDistrito })
                    );
                });
                distritoSelect.selectpicker('refresh');
            });
        }
    });
});

$(document).ready(function () {
    // Llenar el select con los clientes
    $.getJSON('/DireccionAtencion/GetClientes', function (data) {
        let clienteSelect = $('#clienteDireccion');
        clienteSelect.empty(); // Limpia el select
        clienteSelect.append('<option value="">Seleccione el cliente</option>'); // Opción por defecto
        data.forEach(function (item) {
            clienteSelect.append(
                $('<option>', { value: item.idCliente, text: item.nombre })
            );
        });
        clienteSelect.selectpicker('refresh'); // Refresca el selectpicker
    });
});



    //CREAR DIRECCION DE ATENCION

document.getElementById('saveChangesBtn').addEventListener('click', function () {


    const departamentoDireccion = document.getElementById("departamentoDireccion").value;
    const provinciaDireccion = document.getElementById("provinciaDireccion").value;
    const distritoDireccion = document.getElementById("distritoDireccion").value;
    const coordenadas = document.getElementById("coordenadas").value;
    const ventanaatencion = document.getElementById("ventanaatencion").value;
    const clienteDireccion = document.getElementById("clienteDireccion").value;
    const codigopostal = document.getElementById("codigopostal").value;
    const urb = document.getElementById("urb").value;
    const nro = document.getElementById("nro").value;
    const via = document.getElementById("via").value;
    const nombrecalle = document.getElementById("nombrecalle").value;
   


    



    // Hacer el registro en la base de datos
    fetch('/DireccionAtencion/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({

            viaDireccion: via,
            calleDireccion: nombrecalle,
            nroDireccion: nro,
            urbDireccion: urb,
            codpostalDireccion: codigopostal,
            coordenadasDireccion: coordenadas,
            ventanaAtencionDireccion: ventanaatencion,
            idDepartamento: departamentoDireccion || null,
            idProvincia: provinciaDireccion || null,
            idDistrito: distritoDireccion || null,
            idCliente: clienteDireccion

         
            
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

             
            } else {
                // Si hubo un error en la respuesta del servidor
                Swal.fire({
                    icon: 'error',
                    title: 'Error al registrar',
                    text: 'Hubo un problema al registrar a la Direccion de Atencion'
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


//eliminar direccion

$(document).ready(function () {
    // Botón eliminar usuario
    $(".btn-eliminar-direccion").on("click", function () {
        const idDireccion = $(this).data("id");

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
                    url: `/DireccionAtencion/Eliminar/${idDireccion}`,
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


//INFO DIRECCION


function abrirModalDireccionAtencion(button) {
    const id = button.getAttribute("data-id");

    fetch(`/DireccionAtencion/ObtenerDireccionesAtencion?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos de la direccion.");
            }
            return response.json();
        })
        .then(direccionesAtencion => {
            // Verifica el contenido de los datos recibidos
            console.log(direccionesAtencion);

            // Asigna los valores al modal

            document.getElementById("modalViaDireccion").textContent = direccionesAtencion.viaDireccion;
            document.getElementById("modalCalleDireccion").textContent = direccionesAtencion.calleDireccion;
            document.getElementById("modalNroDireccion").textContent = direccionesAtencion.nroDireccion;
            document.getElementById("modalUrbDireccion").textContent = direccionesAtencion.urbDireccion;
            document.getElementById("modalCodpostalDireccion").textContent = direccionesAtencion.codpostalDireccion;
            document.getElementById("modalCoordenadasDireccion").textContent = direccionesAtencion.coordenadasDireccion;
            document.getElementById("modalDepartamento").textContent = direccionesAtencion.departamento;
            document.getElementById("modalProvincia").textContent = direccionesAtencion.provincia;
            document.getElementById("modalDistrito").textContent = direccionesAtencion.distrito;
            document.getElementById("modalClienteNombre").textContent = direccionesAtencion.cliente;




           
            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalDireccionAtencionDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información del usuario.");
        });
}