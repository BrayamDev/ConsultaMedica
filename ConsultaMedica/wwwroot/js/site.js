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
        locale: 'es',
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
        allDayText: '',
        dayHeaderFormat: { weekday: 'long' },
        views: {
            dayGridMonth: {
                titleFormat: { year: 'numeric', month: 'long' }
            },
            timeGridWeek: {
                titleFormat: { year: 'numeric', month: 'long', day: 'numeric' },
                slotLabelFormat: {
                    hour: 'numeric',
                    minute: '2-digit',
                    hour12: true,
                    meridiem: 'short'
                }
            },
            timeGridDay: {
                titleFormat: { year: 'numeric', month: 'long', day: 'numeric', weekday: 'long' },
                slotLabelFormat: {
                    hour: 'numeric',
                    minute: '2-digit',
                    hour12: true,
                    meridiem: 'short'
                }
            }
        },
        eventTimeFormat: {
            hour: 'numeric',
            minute: '2-digit',
            hour12: true,
            meridiem: 'short'
        },
        events: function (fetchInfo, successCallback, failureCallback) {
            fetch('/Agenda/GetCitas')
                .then(response => response.json())
                .then(data => {
                    successCallback(data);
                })
                .catch(error => {
                    console.error('Error al cargar las citas:', error);
                    failureCallback('Error al cargar las citas');
                });
        },
        eventClick: function (info) {
            if (info.jsEvent.detail === 2) { // Detectar doble clic
                // Mostrar el modal
                var modal = new bootstrap.Modal(document.getElementById('opcionesCitaModal'));
                modal.show();
            }
        },
        eventContent: function (arg) {
            var observaciones = arg.event.extendedProps.observaciones;
            var content = document.createElement('div');
            content.classList.add('event-container');

            // Hora del evento en vistas de Semana/Día
            if (arg.view.type !== 'dayGridMonth') {
                var time = document.createElement('div');
                time.classList.add('event-time');
                time.innerHTML = `<strong>${arg.timeText}</strong>`;
                content.appendChild(time);
            }

            // Título del evento
            var title = document.createElement('div');
            title.classList.add('event-title');
            title.innerHTML = arg.event.title;
            content.appendChild(title);

            // Observaciones (si existen)
            if (observaciones) {
                var obs = document.createElement('div');
                obs.classList.add('event-observations');
                obs.innerHTML = observaciones;
                content.appendChild(obs);
            }

            return { domNodes: [content] };
        },
        eventDidMount: function (info) {
            // Eliminar las clases no deseadas
            const classesToRemove = [
                'fc-timegrid-event',
                'fc-v-event',
                'fc-timegrid-event-short',
                'fc-event-start',
                'fc-event-end',
                'fc-event-past'
            ];

            classesToRemove.forEach(className => {
                info.el.classList.remove(className);
            });
        }
    });

    calendar.render();
});
function abrirModalEditarCita(evento) {
    const cita = {
        id: evento.id,
        title: evento.title,
        start: evento.start,
        end: evento.end,
        extendedProps: evento.extendedProps
    };

    document.getElementById('citaId').value = cita.id;
    document.getElementById('estadoCita').value = cita.extendedProps.estado || 'Activa';
    document.getElementById('fechaHora').value = cita.start.toISOString().slice(0, 16);
    document.getElementById('especialidad').value = cita.extendedProps.especialidad || 'Medicina General';
    document.getElementById('tratamiento').value = cita.extendedProps.tratamiento || '';
    document.getElementById('tiempoVisita').value = (cita.end - cita.start) / (1000 * 60);
    document.getElementById('paciente').value = cita.extendedProps.paciente || '';
    document.getElementById('tarifa').value = cita.extendedProps.tarifa || '';
    document.getElementById('observaciones').value = cita.extendedProps.observaciones || '';
    document.getElementById('destaca').checked = cita.extendedProps.destaca || false;
    document.getElementById('citaMultiple').checked = cita.extendedProps.citaMultiple || false;

    const modal = new bootstrap.Modal(document.getElementById('editarCitaModal'));
    modal.show();
}


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
