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
        }
    });

    calendar.render();
});
/* **************************************** FULL CALENDAR en nueva ANGENDA ****************************************** */