/* **************************************** Busqueda de pacientes en nueva cita ****************************************** */
document.addEventListener("DOMContentLoaded", function () {
    const buscarPaciente = document.getElementById("buscarPaciente");
    const listaPacientes = document.getElementById("listaPacientes");
    const inputPaciente = document.getElementById("pacienteNombre");
    const hiddenPacienteId = document.getElementById("pacienteId");
    const modalPacientes = new bootstrap.Modal(document.getElementById("modalPacientes")); // Inicializa el modal

    // Función para normalizar texto y eliminar tildes
    function normalizarTexto(texto) {
        return texto.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase();
    }

    // Filtrar la lista de pacientes en el modal
    buscarPaciente.addEventListener("input", function () {
        const filtro = normalizarTexto(buscarPaciente.value);

        document.querySelectorAll(".paciente-item").forEach(item => {
            const texto = normalizarTexto(item.textContent);
            item.style.display = texto.includes(filtro) ? "block" : "none";
        });
    });

    // Permitir seleccionar un paciente y cerrar el modal
    document.addEventListener("click", function (e) {
        if (e.target.classList.contains("paciente-item")) {
            const pacienteId = e.target.getAttribute("data-id");
            const pacienteNombre = e.target.textContent;

            // Asignar valores al input y al campo oculto
            inputPaciente.value = pacienteNombre;
            hiddenPacienteId.value = pacienteId;

            // Cerrar el modal
            modalPacientes.hide();
        }
    });
});

/* **************************************** Busqueda de pacientes en nueva cita ****************************************** */


/* **************************************** FULL CALENDAR en nueva ANGENDA ****************************************** */
document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',  // Esto traduce automáticamente los nombres de meses y días
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día'
        },
        allDayText: '', // Elimina el texto "Todo el día"
        dayHeaderFormat: { weekday: 'long' },  // Asegura que los días se muestren completos con la primera en mayúscula
        views: {
            dayGridMonth: {
                titleFormat: { year: 'numeric', month: 'long' } // Mes con primera en mayúscula
            },
            timeGridWeek: {
                titleFormat: { year: 'numeric', month: 'long', day: 'numeric' }, // Semana con mes y día en español
                slotLabelFormat: { // Formato de 12 horas para la vista de semana
                    hour: 'numeric',
                    minute: '2-digit',
                    hour12: true,
                    meridiem: 'short'
                }
            },
            timeGridDay: {
                titleFormat: { year: 'numeric', month: 'long', day: 'numeric', weekday: 'long' }, // Día con nombre completo
                slotLabelFormat: { // Formato de 12 horas para la vista de día
                    hour: 'numeric',
                    minute: '2-digit',
                    hour12: true,
                    meridiem: 'short'
                }
            }
        },
        eventTimeFormat: { // Formato de 12 horas para los eventos
            hour: 'numeric',
            minute: '2-digit',
            hour12: true,
            meridiem: 'short'
        },
        events: function (fetchInfo, successCallback, failureCallback) {
            // Hacer una solicitud AJAX para obtener las citas
            fetch('/Agenda/GetCitas') // Ruta al método del controlador
                .then(response => response.json())
                .then(data => {
                    // Pasar los datos a FullCalendar
                    successCallback(data);
                })
                .catch(error => {
                    console.error('Error al cargar las citas:', error);
                    failureCallback('Error al cargar las citas');
                });
        },
        eventClick: function (info) {
            // Detectar doble clic
            if (info.jsEvent.detail === 2) { // Si se hizo doble clic
                abrirModalOpcionesCita(info.event); // Abrir el nuevo modal con opciones
            }
        }
    });

    calendar.render();
});

// Nueva función para abrir el modal con las tres opciones
function abrirModalOpcionesCita(evento) {
    // Guardar el evento en una variable global para usarlo más tarde
    window.eventoSeleccionado = evento;

    // Mostrar el modal de opciones
    const modal = new bootstrap.Modal(document.getElementById('opcionesCitaModal'));
    modal.show();
}

// Mantenemos la función original para editar cita, pero la llamaremos desde el botón "Modificar Cita"
function abrirModalEditarCita(evento) {
    // Obtener los datos de la cita
    const cita = {
        id: evento.id,
        title: evento.title,
        start: evento.start,
        end: evento.end,
        extendedProps: evento.extendedProps // Propiedades adicionales (observaciones, pacienteId, etc.)
    };

    // Llenar el formulario del modal con los datos de la cita
    document.getElementById('citaId').value = cita.id;
    document.getElementById('estadoCita').value = cita.extendedProps.estado || 'Activa'; // Estado de la cita
    document.getElementById('fechaHora').value = cita.start.toISOString().slice(0, 16); // Formato datetime-local
    document.getElementById('especialidad').value = cita.extendedProps.especialidad || 'Medicina General';
    document.getElementById('tratamiento').value = cita.extendedProps.tratamiento || '';
    document.getElementById('tiempoVisita').value = (cita.end - cita.start) / (1000 * 60); // Duración en minutos
    document.getElementById('paciente').value = cita.extendedProps.paciente || '';
    document.getElementById('tarifa').value = cita.extendedProps.tarifa || '';
    document.getElementById('observaciones').value = cita.extendedProps.observaciones || '';
    document.getElementById('destaca').checked = cita.extendedProps.destaca || false;
    document.getElementById('citaMultiple').checked = cita.extendedProps.citaMultiple || false;

    // Mostrar el modal
    const modal = new bootstrap.Modal(document.getElementById('editarCitaModal'));
    modal.show();
}

// Función para abrir el modal de facturación
function abrirModalFacturacion(evento) {
    // Aquí implementarías la lógica para mostrar el modal de facturación
    alert('Función de facturación para la cita: ' + evento.title);
    // En lugar del alert, mostrarías tu modal de facturación
}

// Función para abrir el modal de historia clínica
function abrirModalHistoriaClinica(evento) {
    // Aquí implementarías la lógica para mostrar el modal de historia clínica
    alert('Abriendo historia clínica del paciente: ' + evento.extendedProps.paciente);
    // En lugar del alert, mostrarías tu modal de historia clínica
}

// Configurar los manejadores de eventos para los botones del modal de opciones
document.addEventListener('DOMContentLoaded', function () {
    // Botón de Facturación
    document.getElementById('btnFacturacion').addEventListener('click', function () {
        const modal = bootstrap.Modal.getInstance(document.getElementById('opcionesCitaModal'));
        modal.hide();
        abrirModalFacturacion(window.eventoSeleccionado);
    });

    // Botón de Modificar Cita
    document.getElementById('btnModificarCita').addEventListener('click', function () {
        const modal = bootstrap.Modal.getInstance(document.getElementById('opcionesCitaModal'));
        modal.hide();
        abrirModalEditarCita(window.eventoSeleccionado);
    });

    // Botón de Historia Clínica
    document.getElementById('btnHistoriaClinica').addEventListener('click', function () {
        const modal = bootstrap.Modal.getInstance(document.getElementById('opcionesCitaModal'));
        modal.hide();
        abrirModalHistoriaClinica(window.eventoSeleccionado);
    });
});


/* **************************************** FULL CALENDAR en nueva ANGENDA ****************************************** */


/* **************************************** INTEL TEL INPUT EN PACIENTE ****************************************** */
// Inicializar intl-tel-input
const input = document.querySelector("#telefono");
window.intlTelInput(input, {
    initialCountry: "auto", // Detectar país automáticamente
    geoIpLookup: function (callback) {
        fetch("https://ipapi.co/json")
            .then(response => response.json())
            .then(data => callback(data.country_code))
            .catch(() => callback("us")); // País por defecto si falla la detección
    },
    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js", // Utilidades
});

/* **************************************** INTEL TEL INPUT EN PACIENTE ****************************************** */

/* **************************************** FICHA DEL DOCTOR ****************************************** */


document.getElementById('fotografia').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('image-preview').src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
});

// Script para mostrar la imagen seleccionada en el campo de tipo file
document.getElementById('fotografia').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('image-preview').src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
});


/* ****************************************  FICHA DEL DOCTOR  ****************************************** */
