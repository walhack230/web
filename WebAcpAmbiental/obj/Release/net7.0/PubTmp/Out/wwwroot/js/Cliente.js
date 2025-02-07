$(function () {
    $("#example1").DataTable({
        "responsive": true, "lengthChange": false, "autoWidth": false,
        "buttons": ["excel", "pdf"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

});

$(document).ready(function () {
    // Evento cuando cambia el valor del select de tipo de documento
    $('#tipoDocumentoCliente').on('change', function () {
        // Obtener el valor seleccionado
        const tipoDocumento = $(this).val();

        // Comprobar el tipo de documento seleccionado
        if (tipoDocumento === 'ce') {
            // Si el documento es "CE" (Carnet de Extranjería), habilitar los campos
            $('#nombresCliente, #apellidoPaternoCliente, #apellidoMaternoCliente').prop('readonly', false);
        } else {
            // Si el documento no es "CE", deshabilitar los campos
            $('#nombresCliente, #apellidoPaternoCliente, #apellidoMaternoCliente').prop('readonly', true);
        }
    });

    // Inicializar el comportamiento cuando se carga la página
    const tipoDocumentoInicial = $('#tipoDocumentoCliente').val();
    if (tipoDocumentoInicial === 'ce') {
        $('#nombresCliente, #apellidoPaternoCliente, #apellidoMaternoCliente').prop('readonly', false);
    } else {
        $('#nombresCliente, #apellidoPaternoCliente, #apellidoMaternoCliente').prop('readonly', true);
    }
});


$(document).ready(function () {
    // Evento cuando cambia el valor del select de tipo de documento
    $('#tipodeDocumentoRepresentante').on('change', function () {
        // Obtener el valor seleccionado
        const tipoDocumento = $(this).val();

        // Comprobar el tipo de documento seleccionado
        if (tipoDocumento === 'ce') {
            // Si el documento es "CE" (Carnet de Extranjería), habilitar los campos
            $('#nombresRepresentante, #ApellidoPaternoRepresentante, #ApellidoMaternoRepresentante').prop('readonly', false);
        } else {
            // Si el documento no es "CE", deshabilitar los campos
            $('#nombresRepresentante, #ApellidoPaternoRepresentante, #ApellidoMaternoRepresentante').prop('readonly', true);
        }
    });

    // Inicializar el comportamiento cuando se carga la página
    const tipoDocumentoInicial = $('#tipodeDocumentoRepresentante').val();
    if (tipoDocumentoInicial === 'ce') {
        // Si el documento es "CE" (Carnet de Extranjería), habilitar los campos
        $('#nombresRepresentante, #ApellidoPaternoRepresentante, #ApellidoMaternoRepresentante').prop('readonly', false);
    } else {
        // Si el documento no es "CE", deshabilitar los campos
        $('#nombresRepresentante, #ApellidoPaternoRepresentante, #ApellidoMaternoRepresentante').prop('readonly', true);
    }
});


$(document).ready(function () {
    // Evento cuando cambia el valor del select de tipo de documento
    $('#tipoDocumentoContacto').on('change', function () {
        // Obtener el valor seleccionado
        const tipoDocumento = $(this).val();

        // Comprobar el tipo de documento seleccionado
        if (tipoDocumento === 'ce' || tipoDocumento === 'ci') {
            // Si el documento es "CE" (Carnet de Extranjería), habilitar los campos
            $('#nombreContacto, #apellidoPaternoContacto, #apellidoMaternoContacto').prop('readonly', false);
        } else {
            // Si el documento no es "CE", deshabilitar los campos
            $('#nombreContacto, #apellidoPaternoContacto, #apellidoMaternoContacto').prop('readonly', true);
        }
    });

    // Inicializar el comportamiento cuando se carga la página
    const tipoDocumentoInicial = $('#tipoDocumentoContacto').val();
    if (tipoDocumentoInicial === 'ce' || tipoDocumentoInicial === 'ci') {
        // Si el documento es "CE" (Carnet de Extranjería), habilitar los campos
        $('#nombreContacto, #apellidoPaternoContacto, #apellidoMaternoContacto').prop('readonly', false);
    } else {
        // Si el documento no es "CE", deshabilitar los campos
        $('#nombreContacto, #apellidoPaternoContacto, #apellidoMaternoContacto').prop('readonly', true);
    }
});

$(document).ready(function () {
    // Cargar vendedores en el select
    $.getJSON('/Cliente/GetVendedores', function (data) {
        let vendedores = $("#vendedores");
        vendedores.empty().append('<option value="">Seleccione un vendedor</option>');
        $.each(data, function (index, vendedor) {
            vendedores.append(`<option value="${vendedor.idUsuario}">${vendedor.nombreCompleto}</option>`);
        });
    });

    $("#vendedores").change(function () {
        let vendedorId = $(this).val();

        if (vendedorId) {
            // Realiza la solicitud para obtener los datos del vendedor
            $.getJSON(`/Cliente/GetAsignacionesByUserId?userId=${vendedorId}`, function (data) {
                if (data.length > 0) {
                    // Asumimos que solo se obtiene una asignación por usuario
                    let asignacion = data[0]; // Tomamos el primer objeto

                    // Asignamos las descripciones a los campos
                    $("#canalventa").val(asignacion.canalDescripcion || "No asignado");
                    $("#zona").val(asignacion.zonaDescripcion || "No asignado");
                    $("#ruta").val(asignacion.rutaDescripcion || "No asignado");

                   
                    // Asignamos el idAsignacion al campo oculto
                    $("#idasignacion").val(asignacion.idAsignacion || "");

                    // Verifica si el idAsignacion se asigna correctamente
                    console.log("ID Asignación para el jefe:", document.getElementById("idasignacion").value);
                } else {
                    // Si no hay datos para este vendedor
                    $("#canalventa, #zona, #ruta").val('No asignado');
                    $("#idasignacion").val(''); // Limpiar el campo oculto
                }
            });
        } else {
            // Limpiar campos si no hay vendedor seleccionado
            $("#canalventa, #zona, #ruta").val('');
            $("#idasignacion").val(''); // Limpiar el campo oculto
        }
    });

});

// Inicializar el código
let codigo = 1;

// Función para obtener el código con formato de 4 dígitos
function getCodigoConFormato() {
    return codigo.toString().padStart(4, '0');
}

// Establecer el código inicial al cargar la página
document.getElementById('codigoCliente').value = getCodigoConFormato();
// CREACION DEL USUARIO

// Escuchar el evento de clic en el botón de "Save changes"
document.getElementById('saveChangesBtn').addEventListener('click', function () {


    const codigoCliente = document.getElementById("codigoCliente").value;
    const tipoCliente = document.getElementById("tipoCliente").value;
    const tipoDocumentoCliente = document.getElementById("tipoDocumentoCliente").value;
    const numeroDocumentoCliente = document.getElementById("numeroDocumentoCliente").value;
    const nombresCliente = document.getElementById("nombresCliente").value;
    const apellidoPaternoCliente = document.getElementById("apellidoPaternoCliente").value;
    const apellidoMaternoCliente = document.getElementById("apellidoMaternoCliente").value;
    const razonSocialCliente = document.getElementById("razonSocialCliente").value;
    const viaDireccion = document.getElementById("viaDireccion").value;
    const nombreDireccion = document.getElementById("nombreDireccion").value;
    const numeroDireccion = document.getElementById("numeroDireccion").value;
    const urbanizacionCliente = document.getElementById("urbanizacionCliente").value;
    const codigoPostal = document.getElementById("codigoPostal").value;
    const telefonoCliente = document.getElementById("telefonoCliente").value;
    const celularCliente = document.getElementById("celularCliente").value;
    const emailCliente = document.getElementById("emailCliente").value;
    const tipodeDocumentoRepresentante = document.getElementById("tipodeDocumentoRepresentante").value;
    const documentoDniRepresentante = document.getElementById("documentoDniRepresentante").value;
    const nombresRepresentante = document.getElementById("nombresRepresentante").value;
    const ApellidoPaternoRepresentante = document.getElementById("ApellidoPaternoRepresentante").value;
    const ApellidoMaternoRepresentante = document.getElementById("ApellidoMaternoRepresentante").value;
    const cargoRepresentante = document.getElementById("cargoRepresentante").value;
    const telefonoRepresentante = document.getElementById("telefonoRepresentante").value;
    const celularRepresentante = document.getElementById("celularRepresentante").value;
    const empresarialemailRepresentante = document.getElementById("empresarialemailRepresentante").value;
    const personalemailRepresentante = document.getElementById("personalemailRepresentante").value;
    const tipoDocumentoContacto = document.getElementById("tipoDocumentoContacto").value;
    const documentNumberContacto = document.getElementById("documentNumberContacto").value;
    const nombreContacto = document.getElementById("nombreContacto").value;
    const apellidoPaternoContacto = document.getElementById("apellidoPaternoContacto").value;
    const apellidoMaternoContacto = document.getElementById("apellidoMaternoContacto").value;
    const cargoContacto = document.getElementById("cargoContacto").value;
    const telefonoContacto = document.getElementById("telefonoContacto").value;
    const celularContacto = document.getElementById("celularContacto").value;
    const empresarialemailContacto = document.getElementById("empresarialemailContacto").value;
    const personalemailContacto = document.getElementById("personalemailContacto").value;
    const dptoCliente = document.getElementById("dptoCliente").value;
    const provinciaCliente = document.getElementById("provinciaCliente").value;
    const distritoCliente = document.getElementById("distritoCliente").value;
    const actividadPrincipalCliente = document.getElementById("actividadPrincipalCliente").value;
    const ventanaAtencion = document.getElementById("ventanaAtencion").value;
    const tipocontacto = document.getElementById("tipocontacto").value;
   
    // Validar el elemento 'idasignacion'
    const asignacionElement = document.getElementById("idasignacion");
    const asignacion = asignacionElement && asignacionElement.value ? asignacionElement.value : null;






    // Hacer el registro en la base de datos
    fetch('/Cliente/Create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            codigoCliente: codigoCliente,
            tipoCliente: tipoCliente || null ,
            documentoCliente: tipoDocumentoCliente || null,  // Usar null si no se proporciona
            nrodocumentoCliente: numeroDocumentoCliente || null,
            nombreCliente: nombresCliente || null,
            apellidopCliente: apellidoPaternoCliente || null,
            apellidomCliente: apellidoMaternoCliente || null,
            razonsocialCliente: razonSocialCliente || null,
            viadireccionCliente: viaDireccion || null,
            calledireccionCliente: nombreDireccion || null,
            nrodireccionCliente: numeroDireccion || null,
            urbdireccionCliente: urbanizacionCliente || null,
            codpostaldireccionCliente: codigoPostal || null,
            telefonoCliente: telefonoCliente || null,
            celularCliente: celularCliente || null,
            emailCliente: emailCliente || null,
            documentoRepresentante: tipodeDocumentoRepresentante || null,
            nrodocumentoRepresentante: documentoDniRepresentante || null,
            nombreRepresentante: nombresRepresentante || null,
            apellidopRepresentante: ApellidoPaternoRepresentante || null,
            apellidomRepresentante: ApellidoMaternoRepresentante || null,
            cargoRepresentante: cargoRepresentante || null,
            telefonoRepresentante: telefonoRepresentante || null,
            celularRepresentante: celularRepresentante || null,
            emailempresarialRepresentante: empresarialemailRepresentante || null,
            emailpersonalRepresentante: personalemailRepresentante || null,
            documentoContacto: tipoDocumentoContacto || null,
            nrodocumentoContacto: documentNumberContacto || null,
            nombreContacto: nombreContacto || null,
            apellidopContacto: apellidoPaternoContacto || null,
            apellidomContacto: apellidoMaternoContacto || null,
            cargoContacto: cargoContacto || null,
            telefonoContacto: telefonoContacto || null,
            celularContacto: celularContacto || null,
            emailempresarialContacto: empresarialemailContacto || null,
            emailpersonalContacto: personalemailContacto || null,
            idDepartamento: dptoCliente || null,
            idProvincia: provinciaCliente || null,
            idDistrito: distritoCliente || null,
            idAsignacion: asignacion,
            actividadPrincipalCliente: actividadPrincipalCliente,
            ventanaAtencionCliente: ventanaAtencion,
            tipocontacto: tipocontacto

            //descripcion: rutas,
            //idEstado: estado === "activo" ? 1 : 2
        })
    })
       .then(response => response.json())
.then(data => {
    if (data.success) {
        Swal.fire({
            icon: 'success',
            title: 'Registro exitoso',
            text: 'El cliente se registró correctamente. ¿Deseas registrar direcciones de atención para este cliente?',
            showCancelButton: true,
            confirmButtonText: 'Sí, registrar',
            cancelButtonText: 'No, gracias'
        }).then((result) => {
            if (result.isConfirmed) {
                // Redirige a la vista DireccionAtencion
                window.location.href = '/DireccionAtencion/DireccionAtencion'; // Ajusta la ruta si es necesario
            } else {
                Swal.fire({
                    icon: 'info',
                    title: 'Proceso finalizado',
                    text: 'El registro se completó sin añadir direcciones de atención.',
                    showConfirmButton: false,
                    timer: 3000
                }).then(() => {
                    location.reload(); // Recarga la página
                });
            }
        });


                // Limpiar los campos del modal
                $('#modal-default').modal('hide');
            } else {
                // Manejar errores específicos
                if (data.message === "El DNI o RUC ya está registrado.") {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Advertencia',
                        text: 'El DNI o RUC ingresado ya está registrado. Por favor, verifique los datos.',
                    });
                } else {
                    // Otros errores genéricos
                    Swal.fire({
                        icon: 'error',
                        title: 'Error al registrar',
                        text: 'Hubo un problema al registrar al cliente. Intente nuevamente más tarde.',
                    });
                }
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
    // Delegación de eventos para los botones de eliminación
    $("#example1").on("click", ".btn-eliminar-cliente", function () {
        const idCliente = $(this).data("id");

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
                // Llamada AJAX para eliminar el cliente
                $.ajax({
                    url: `/Cliente/Eliminar/${idCliente}`,
                    type: "POST",
                    success: function (response) {
                        if (response.success) {
                            Swal.fire("¡Eliminado!", "El cliente ha sido eliminado.", "success")
                                .then(() => location.reload()); // Recargar la página
                        } else {
                            Swal.fire("Error", response.message || "No se pudo eliminar el cliente.", "error");
                        }
                    },
                    error: function () {
                        Swal.fire("Error", "Ocurrió un problema al intentar eliminar el cliente.", "error");
                    }
                });
            }
        });
    });
});




function abrirModalCliente(button) {
    const id = button.getAttribute("data-id");

    fetch(`/Cliente/ObtenerMaestroCliente?id=${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al obtener los datos del Cliente.");
            }
            return response.json();
        })
        .then(maestrocliente => {
            // Verifica el contenido de los datos recibidos
            console.log(maestrocliente);

            // Asigna los valores al modal

            document.getElementById("modalCodigo").textContent = maestrocliente.codigoCliente;

            document.getElementById("modalTipoCliente").textContent = maestrocliente.tipoCliente;

            document.getElementById("modalDocumento").textContent = maestrocliente.documentoCliente;

            document.getElementById("modalNroDocumento").textContent = maestrocliente.nrodocumentoCliente;

            document.getElementById("modalNombresCliente").textContent = maestrocliente.nombreCliente;

            document.getElementById("modalApellidopCliente").textContent = maestrocliente.apellidopCliente;

            document.getElementById("modalApellidomCliente").textContent = maestrocliente.apellidomCliente;

            document.getElementById("modalRazonSocialCliente").textContent = maestrocliente.razonsocialCliente;

            document.getElementById("modalTelefonoCliente").textContent = maestrocliente.telefonoCliente;

            document.getElementById("modalCelularCliente").textContent = maestrocliente.celularCliente;

            document.getElementById("modalEmailCliente").textContent = maestrocliente.emailCliente;

            document.getElementById("modalDepartamentoCliente").textContent = maestrocliente.departamento;

            document.getElementById("modalProvinciaCliente").textContent = maestrocliente.provincia;

            document.getElementById("modalDistritoCliente").textContent = maestrocliente.distrito;

            document.getElementById("modalUrbanizacionCliente").textContent = maestrocliente.urbdireccionCliente;

            document.getElementById("modalPostalCliente").textContent = maestrocliente.codpostaldireccionCliente;

            document.getElementById("modalNumeroCliente").textContent = maestrocliente.nrodireccionCliente;

            document.getElementById("modalCalleCliente").textContent = maestrocliente.calledireccionCliente;

            document.getElementById("modalViaCliente").textContent = maestrocliente.viadireccionCliente;

            //REPRESENTANTE
            document.getElementById("modalTipoDocumentoRepresentante").textContent = maestrocliente.documentoRepresentante;


            document.getElementById("modalNroDocumentoRepresentante").textContent = maestrocliente.nrodocumentoRepresentante;

            document.getElementById("modalNombresRepresentante").textContent = maestrocliente.nombreRepresentante;

            document.getElementById("modalApellidoPaternoRepresentante").textContent = maestrocliente.apellidopRepresentante;

            document.getElementById("modalApellidoMaternoRepresentante").textContent = maestrocliente.apellidomRepresentante;

            document.getElementById("modalCargoRepresentante").textContent = maestrocliente.cargoRepresentante;

            document.getElementById("modalTelefonoRepresentante").textContent = maestrocliente.telefonoRepresentante;

            document.getElementById("modalCelularRepresentante").textContent = maestrocliente.celularRepresentante;

            document.getElementById("modalEmailPersonalRepresentante").textContent = maestrocliente.emailpersonalRepresentante;

            document.getElementById("modalEmailEmpresarialRepresentante").textContent = maestrocliente.emailempresarialRepresentante;

            //CONTACTO


            document.getElementById("modalTipoDocumentoContacto").textContent = maestrocliente.documentoContacto;


            document.getElementById("modalNroDocumentoContacto").textContent = maestrocliente.nrodocumentoContacto;

            document.getElementById("modalNombresContacto").textContent = maestrocliente.nombreContacto;

            document.getElementById("modalApellidoPaternoContacto").textContent = maestrocliente.apellidopContacto;

            document.getElementById("modalApellidoMaternoContacto").textContent = maestrocliente.apellidomContacto;

            document.getElementById("modalCargoContacto").textContent = maestrocliente.cargoContacto;

            document.getElementById("modalTelefonoContacto").textContent = maestrocliente.telefonoContacto;

            document.getElementById("modalCelularContacto").textContent = maestrocliente.celularContacto;

            document.getElementById("modalEmailPersonalContacto").textContent = maestrocliente.emailpersonalContacto;

            document.getElementById("modalEmailEmpresarialContacto").textContent = maestrocliente.emailempresarialContacto;


            document.getElementById("modalCanalVenta").textContent = maestrocliente.canal;


            document.getElementById("modalZona").textContent = maestrocliente.zona;

            document.getElementById("modalRuta").textContent = maestrocliente.ruta;

            document.getElementById("modalVendedor").textContent = maestrocliente.usuario;

            //nuevos campos


            document.getElementById("modalActividadPrincipal").textContent = maestrocliente.actividadPrincipalCliente;
            document.getElementById("modalVentana").textContent = maestrocliente.ventanaAtencionCliente;

            document.getElementById("modalTipo").textContent = maestrocliente.tipoContacto;









            // Muestra el modal
            const modal = new bootstrap.Modal(document.getElementById("modalClienteDetalle"));
            modal.show();
        })
        .catch(error => {
            console.error(error);
            alert("No se pudo obtener la información del usuario.");
        });
}



function editarCliente(button) {
    const idCliente = button.getAttribute("data-id");

    fetch(`/Cliente/ObtenerMaestroCliente?id=${idCliente}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Cliente no encontrado");
            }
            return response.json();
        })
        .then(maestrocliente => {
            console.log("Datos del cliente recibidos:", maestrocliente);

       


            document.getElementById("codigoClienteEditar").value = maestrocliente.codigoCliente;
            document.getElementById("tipoClienteEditar").value = maestrocliente.tipoCliente;
            document.getElementById("tipoDocumentoClienteEditar").value = maestrocliente.documentoCliente;
            document.getElementById("numeroDocumentoClienteEditar").value = maestrocliente.nrodocumentoCliente;
           
            document.getElementById("nombresClienteEditar").value = maestrocliente.nombreCliente;
            document.getElementById("apellidoPaternoClienteEditar").value = maestrocliente.apellidopCliente;
            document.getElementById("apellidoMaternoClienteEditar").value = maestrocliente.apellidomCliente;
            document.getElementById("telefonoClienteEditar").value = maestrocliente.telefonoCliente;
            document.getElementById("celularClienteEditar").value = maestrocliente.celularCliente;
            document.getElementById("emailClienteEditar").value = maestrocliente.emailCliente;
            document.getElementById("razonSocialClienteEditar").value = maestrocliente.razonsocialCliente;
            document.getElementById("actividadPrincipalClienteEditar").value = maestrocliente.actividadPrincipalCliente;
            document.getElementById("dptoClienteEditar").value = maestrocliente.idDepartamento;
            // Cargar provincias basadas en el departamento
            cargarProvincias(maestrocliente.idDepartamento).then(() => {
                document.getElementById("provinciaClienteEditar").value = maestrocliente.idProvincia;

                // Cargar distritos basados en la provincia
                return cargarDistritos(maestrocliente.idProvincia);
            }).then(() => {
                document.getElementById("distritoClienteEditar").value = maestrocliente.idDistrito;
            }).catch(error => {
                console.error("Error al cargar provincias o distritos:", error);
                alert("Error al cargar los datos de ubicación.");
            });
            document.getElementById("urbanizacionClienteEditar").value = maestrocliente.urbdireccionCliente;
            document.getElementById("codigoPostalEditar").value = maestrocliente.codpostaldireccionCliente;
            document.getElementById("numeroDireccionEditar").value = maestrocliente.nrodireccionCliente;
            document.getElementById("nombreDireccionEditar").value = maestrocliente.calledireccionCliente;
            document.getElementById("viaDireccionEditar").value = maestrocliente.viadireccionCliente;
            document.getElementById("ventanaAtencionEditar").value = maestrocliente.ventanaAtencionCliente;

            //REPRESENTANTE

            document.getElementById("tipodeDocumentoRepresentanteEditar").value = maestrocliente.documentoRepresentante;
            document.getElementById("documentoDniRepresentanteEditar").value = maestrocliente.nrodocumentoRepresentante;
            document.getElementById("ApellidoPaternoRepresentanteEditar").value = maestrocliente.apellidopRepresentante;
            document.getElementById("ApellidoMaternoRepresentanteEditar").value = maestrocliente.apellidomRepresentante;
            document.getElementById("nombresRepresentanteEditar").value = maestrocliente.nombreRepresentante;
            document.getElementById("cargoRepresentanteEditar").value = maestrocliente.cargoRepresentante;
            document.getElementById("telefonoRepresentanteEditar").value = maestrocliente.telefonoRepresentante;
            document.getElementById("celularRepresentanteEditar").value = maestrocliente.celularRepresentante;
            document.getElementById("personalemailRepresentanteEditar").value = maestrocliente.emailpersonalRepresentante;
            document.getElementById("empresarialemailRepresentanteEditar").value = maestrocliente.emailempresarialRepresentante;


            //CONTACTO

            document.getElementById("tipoDocumentoContactoEditar").value = maestrocliente.documentoContacto;
            document.getElementById("documentNumberContactoEditar").value = maestrocliente.nrodocumentoContacto;
            document.getElementById("apellidoPaternoContactoEditar").value = maestrocliente.apellidopContacto;
            document.getElementById("apellidoMaternoContactoEditar").value = maestrocliente.apellidomContacto;
            document.getElementById("nombreContactoEditar").value = maestrocliente.nombreContacto;
            document.getElementById("tipocontactoEditar").value = maestrocliente.tipoContacto;
            document.getElementById("cargoContactoEditar").value = maestrocliente.cargoContacto;
            document.getElementById("telefonoContactoEditar").value = maestrocliente.telefonoContacto;
            document.getElementById("celularContactoEditar").value = maestrocliente.celularContacto;
            document.getElementById("personalemailContactoEditar").value = maestrocliente.emailpersonalContacto;
            document.getElementById("empresarialemailContactoEditar").value = maestrocliente.emailempresarialContacto;



            // Información comercial (Vendedor, Canal, Zona, Ruta)
            document.getElementById("idasignacionEditar").value = maestrocliente.idAsignacion;
            document.getElementById("vendedoresEditar").value = maestrocliente.usuario;
            document.getElementById("canalventaEditar").value = maestrocliente.canal;
            document.getElementById("zonaEditar").value = maestrocliente.zona;
            document.getElementById("rutaEditar").value = maestrocliente.ruta;

            // Cargar información de acuerdo al vendedor
            document.getElementById("vendedoresEditar").addEventListener("change", function () {
                const idAsignacion = this.value;

                if (idAsignacion) {
                    const asignacion = maestrocliente.idAsignacionNavigation.find(a => a.id === idAsignacion);
                } else {
                    console.error("idAsignacion no tiene un valor válido");
                }


                if (asignacion) {
                    document.getElementById("canalventaEditar").value = asignacion.idCanalNavigation.descripcion;
                    document.getElementById("zonaEditar").value = asignacion.idZonaNavigation.descripcion;
                    document.getElementById("rutaEditar").value = asignacion.idRutaNavigation.descripcion;
                }
            });

            //// Corregir el nombre de la propiedad de la contraseña
            //document.getElementById("contraseñaEditar").value = usuario.passwordUsuario;

            //// Configurar el valor del select para Rol
            //document.getElementById("rolEditar").value = usuario.idRol;

            //// Configurar el valor del select para Estado
            //document.getElementById("estadoEditar").value = usuario.idEstado;

            //// Guardar el ID en un atributo oculto
            document.getElementById("codigoClienteEditar").dataset.id = maestrocliente.idCliente;

            // Abrir el modal
            const modal = new bootstrap.Modal(document.getElementById("modalEditarCliente"));
            modal.show();
        })
        .catch(error => {
            console.error("Error al cargar los datos del usuario:", error);
            alert("Error al cargar los datos del usuario");
        });
}









// Función para cargar provincias
function cargarProvincias(departamentoId) {
    const provinciaSelect = document.getElementById("provinciaClienteEditar");
    provinciaSelect.innerHTML = '<option value="">Seleccione una provincia</option>';
    provinciaSelect.disabled = true;

    return fetch(`/Cliente/GetProvincias?departamentoId=${departamentoId}`)
        .then(response => response.json())
        .then(data => {
            data.forEach(provincia => {
                const option = new Option(provincia.nombreProvincia, provincia.idProvincia);
                provinciaSelect.add(option);
            });
            provinciaSelect.disabled = false;
        });
}

// Función para cargar distritos
function cargarDistritos(provinciaId) {
    const distritoSelect = document.getElementById("distritoClienteEditar");
    distritoSelect.innerHTML = '<option value="">Seleccione un distrito</option>';
    distritoSelect.disabled = true;

    return fetch(`/Cliente/GetDistritos?provinciaId=${provinciaId}`)
        .then(response => response.json())
        .then(data => {
            data.forEach(distrito => {
                const option = new Option(distrito.nombreDistrito, distrito.idDistrito);
                distritoSelect.add(option);
            });
            distritoSelect.disabled = false;
        });
}



document.getElementById("saveChangesBtnEditar").addEventListener("click", function () {
    const maestrocliente = {
        IdCliente: parseInt(document.getElementById("codigoClienteEditar").dataset.id),
        CodigoCliente: document.getElementById("codigoClienteEditar").value,
        TipoCliente: document.getElementById("tipoClienteEditar").value,
        DocumentoCliente: document.getElementById("tipoDocumentoClienteEditar").value,
        NrodocumentoCliente: document.getElementById("numeroDocumentoClienteEditar").value,
        NombreCliente: document.getElementById("nombresClienteEditar").value,
        ApellidopCliente: document.getElementById("apellidoPaternoClienteEditar").value,
        ApellidomCliente: document.getElementById("apellidoMaternoClienteEditar").value,
        RazonsocialCliente: document.getElementById("razonSocialClienteEditar").value,
        ViadireccionCliente: document.getElementById("viaDireccionEditar").value,
        CalledireccionCliente: document.getElementById("nombreDireccionEditar").value,
        NrodireccionCliente: document.getElementById("numeroDireccionEditar").value,
        UrbdireccionCliente: document.getElementById("urbanizacionClienteEditar").value,
        CodpostaldireccionCliente: document.getElementById("codigoPostalEditar").value,
        TelefonoCliente: document.getElementById("telefonoClienteEditar").value,
        CelularCliente: document.getElementById("celularClienteEditar").value,
        EmailCliente: document.getElementById("emailClienteEditar").value,

        // Datos del representante
        DocumentoRepresentante: document.getElementById("tipodeDocumentoRepresentanteEditar").value,
        NrodocumentoRepresentante: document.getElementById("documentoDniRepresentanteEditar").value,
        NombreRepresentante: document.getElementById("nombresRepresentanteEditar").value,
        ApellidopRepresentante: document.getElementById("ApellidoPaternoRepresentanteEditar").value,
        ApellidomRepresentante: document.getElementById("ApellidoMaternoRepresentanteEditar").value,
        CargoRepresentante: document.getElementById("cargoRepresentanteEditar").value,
        TelefonoRepresentante: document.getElementById("telefonoRepresentanteEditar").value,
        CelularRepresentante: document.getElementById("celularRepresentanteEditar").value,
        EmailempresarialRepresentante: document.getElementById("empresarialemailRepresentanteEditar").value,
        EmailpersonalRepresentante: document.getElementById("personalemailRepresentanteEditar").value,

        // Datos del contacto
        DocumentoContacto: document.getElementById("tipoDocumentoContactoEditar").value,
        NrodocumentoContacto: document.getElementById("documentNumberContactoEditar").value,
        NombreContacto: document.getElementById("nombreContactoEditar").value,
        ApellidopContacto: document.getElementById("apellidoPaternoContactoEditar").value,
        ApellidomContacto: document.getElementById("apellidoMaternoContactoEditar").value,
        CargoContacto: document.getElementById("cargoContactoEditar").value,
        TelefonoContacto: document.getElementById("telefonoContactoEditar").value,
        CelularContacto: document.getElementById("celularContactoEditar").value,
        EmailempresarialContacto: document.getElementById("empresarialemailContactoEditar").value,
        EmailpersonalContacto: document.getElementById("personalemailContactoEditar").value,

        // Datos adicionales
        ActividadPrincipalCliente: document.getElementById("actividadPrincipalClienteEditar").value,
        VentanaAtencionCliente: document.getElementById("ventanaAtencionEditar").value,
        TipoContacto: document.getElementById("tipocontactoEditar").value,

        // ID de ubicación
        IdDepartamento: document.getElementById("dptoClienteEditar").value,
        IdProvincia: document.getElementById("provinciaClienteEditar").value,
        IdDistrito: document.getElementById("distritoClienteEditar").value,
        IdAsignacion: document.getElementById("idasignacionEditar").value
    };

    fetch("/Cliente/ActualizarMaestroCliente", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(maestrocliente)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al actualizar el cliente");
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








