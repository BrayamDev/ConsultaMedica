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
                    data.forEach(event => {
                        if (event.extendedProps) {
                            event.title = `${event.extendedProps.nombre || ''} ${event.extendedProps.apellido || ''}`.trim();
                        }
                    });
                    successCallback(data);
                })
                .catch(error => {
                    console.error('Error al cargar las citas:', error);
                    failureCallback('Error al cargar las citas');
                });
        },
        eventContent: function (arg) {
            var observaciones = arg.event.extendedProps.observaciones;
            var content = document.createElement('div');
            content.classList.add('event-container');
            content.style.display = 'flex';
            content.style.alignItems = 'center';
            content.style.gap = '5px';
            content.style.width = '100%';

            // Contenedor de iconos
            var iconContainer = document.createElement('div');
            iconContainer.classList.add('event-icons');
            iconContainer.style.display = 'flex';
            iconContainer.style.flexDirection = 'column';
            iconContainer.style.gap = '3px';
            iconContainer.style.marginRight = '5px';

            // Iconos de acciones
            var facturacionIcon = document.createElement('i');
            facturacionIcon.classList.add('ri-bill-line');
            facturacionIcon.title = 'Facturación';
            facturacionIcon.addEventListener('click', function (e) {
                e.stopPropagation();
                window.location.href = 'Facturacion/Index?id=' + arg.event.id;
            });

            var editarIcon = document.createElement('i');
            editarIcon.classList.add('ri-edit-2-line');
            editarIcon.title = 'Editar cita';
            editarIcon.addEventListener('click', function (e) {
                e.stopPropagation();
                abrirModalEditarCita(arg.event);
            });

            var historiaIcon = document.createElement('i');
            historiaIcon.classList.add('ri-file-user-line');
            historiaIcon.title = 'Historia clínica';
            historiaIcon.addEventListener('click', function (e) {
                e.stopPropagation();
                window.location.href = 'HistoriaClinica/Index?id=' + arg.event.id;
            });

            iconContainer.appendChild(facturacionIcon);
            iconContainer.appendChild(editarIcon);
            iconContainer.appendChild(historiaIcon);

            // Contenido del evento
            var eventContent = document.createElement('div');
            eventContent.classList.add('event-content');
            eventContent.style.overflow = 'hidden';

            // Título (Nombre + Apellido)
            var title = document.createElement('div');
            title.classList.add('event-title');
            title.style.fontWeight = 'bold';
            title.style.whiteSpace = 'nowrap';
            title.style.overflow = 'hidden';
            title.style.textOverflow = 'ellipsis';
            title.textContent = arg.event.title;
            eventContent.appendChild(title);

            // Hora en vistas de semana/día
            if (arg.view.type !== 'dayGridMonth') {
                var time = document.createElement('div');
                time.classList.add('event-time');
                time.style.fontSize = '0.85em';
                time.style.color = '#555';
                time.textContent = arg.timeText;
                eventContent.insertBefore(time, title);
            }

            // Observaciones
            if (observaciones) {
                var obs = document.createElement('div');
                obs.classList.add('event-observations');
                obs.style.fontSize = '0.8em';
                obs.style.color = '#fff';
                obs.style.marginTop = '3px';
                obs.textContent = observaciones;
                eventContent.appendChild(obs);
            }

            content.appendChild(iconContainer);
            content.appendChild(eventContent);

            return { domNodes: [content] };
        },
        eventDidMount: function (info) {
            // Estilos para el evento - fondo blanco y borde
            const eventEl = info.el;
            eventEl.style.borderRadius = '4px';
            eventEl.style.padding = '5px';
            eventEl.style.margin = '2px 0';
            eventEl.style.backgroundColor = '#ffffff'; // Fondo blanco
            eventEl.style.border = '1px solid #e0e0e0'; // Borde gris claro
            eventEl.style.color = '#333333';
            eventEl.style.boxShadow = '0 1px 3px rgba(0,0,0,0.05)';

            // Eliminar clases de FullCalendar
            const classesToRemove = [
                'fc-timegrid-event',
                'fc-v-event',
                'fc-timegrid-event-short',
                'fc-event-start',
                'fc-event-end',
                'fc-event-past',
                'fc-daygrid-event'
            ];

            classesToRemove.forEach(className => {
                eventEl.classList.remove(className);
            });

            // Estilos para los iconos
            const icons = eventEl.querySelectorAll('.event-icons i');
            icons.forEach(icon => {
                icon.style.fontSize = '14px';
                icon.style.padding = '2px';
                icon.style.borderRadius = '3px';
                icon.style.transition = 'all 0.2s';
                icon.style.color = '#fff';
                icon.style.cursor = 'pointer';

                icon.addEventListener('mouseenter', () => {
                    icon.style.color = '#00826F';
                    icon.style.backgroundColor = 'rgba(0, 130, 111, 0.1)';
                });

                icon.addEventListener('mouseleave', () => {
                    icon.style.color = '#fff';
                    icon.style.backgroundColor = 'transparent';
                });
            });
        }
    });

    calendar.render();
});

// Función para abrir modal de edición (se mantiene para el icono de editar)
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
