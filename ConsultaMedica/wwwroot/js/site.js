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
    // Configuración del mini calendario
    var miniCalendarEl = document.getElementById('miniCalendar');
    var miniCalendar = new FullCalendar.Calendar(miniCalendarEl, {
        locale: 'es',
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev',
            center: 'title',
            right: 'next'
        },
        fixedWeekCount: false,
        height: 'auto',
        contentHeight: 'auto',
        aspectRatio: 1,
        dayMaxEventRows: 1,
        dayHeaderFormat: {
            weekday: 'short'
        },
        // Reducir tamaño del texto en los días
        dayCellContent: function (info) {
            info.dayNumberText = info.dayNumberText.replace(/^0/, '');
            return { html: '<small>' + info.dayNumberText + '</small>' };
        },
        // Reducir tamaño del título del mes
        titleFormat: {
            year: 'numeric',
            month: 'short'
        },
        dateClick: function (info) {
            // Prevenir que el dropdown se cierre
            info.jsEvent.stopPropagation();

            // Obtener la instancia del calendario principal
            var mainCalendar = FullCalendar.Calendar.getCalendarById('calendar');

            if (mainCalendar) {
                // Cambiar la vista del calendario principal al día seleccionado
                mainCalendar.gotoDate(info.date);
                mainCalendar.changeView('timeGridDay'); // Cambiar a vista de día

                // Opcional: Cerrar el dropdown después de seleccionar
                var dropdown = bootstrap.Dropdown.getInstance(document.getElementById('miniCalendarDropdown'));
                dropdown.hide();
            }

            // Resaltar la fecha seleccionada
            miniCalendarEl.querySelectorAll('.fc-day').forEach(day => {
                day.classList.remove('fc-day-selected');
            });
            info.dayEl.classList.add('fc-day-selected');
        },
        events: function (fetchInfo, successCallback, failureCallback) {
            fetch('/Agenda/GetCitas')
                .then(response => response.json())
                .then(data => {
                    successCallback(data);
                })
                .catch(error => {
                    console.error('Error al cargar las citas:', error);
                    failureCallback(error);
                });
        },
        eventClick: function (info) {
            // Redirigir a la edición de la cita
            window.location.href = 'Agenda/EditarCitas?id=' + info.event.id;
            info.jsEvent.stopPropagation();
        }
    });

    miniCalendar.render();

    // Actualizar tamaño cuando se abre el dropdown
    document.getElementById('miniCalendarDropdown').addEventListener('shown.bs.dropdown', function () {
        miniCalendar.updateSize();
    });

    // Prevenir que el dropdown se cierre al hacer clic dentro
    document.querySelector('#miniCalendarDropdown + .dropdown-menu').addEventListener('click', function (e) {
        e.stopPropagation();
    });
});


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
        // Agrega este nuevo manejador de eventos
        dateClick: function (info) {
            // Formatea la fecha para el input datetime-local
            var fechaSeleccionada = info.dateStr; // Formato YYYY-MM-DD
            var horaActual = new Date().toTimeString().substring(0, 5); // HH:MM

            // Combina fecha y hora para el input
            var fechaHora = fechaSeleccionada + 'T' + horaActual;

            // Establece el valor en el campo de fecha y abre el modal
            document.getElementById('fechaHora').value = fechaHora;

            // Abre el modal de nueva cita
            var modal = new bootstrap.Modal(document.getElementById('nuevaCitaModal'));
            modal.show();

            // Opcional: puedes cambiar el color de fondo del día seleccionado
            info.dayEl.style.backgroundColor = 'rgba(0, 130, 111, 0.1)';
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
            content.style.flexDirection = 'row'; // Asegura disposición horizontal
            content.style.alignItems = 'center';
            content.style.gap = '3px'; // Reducir espacio entre elementos
            content.style.width = '100%';
            content.style.height = '100%'; // Asegurar que ocupe toda la altura
            content.style.fontSize = '1.1rem'; // Texto más pequeño

            // Contenedor de iconos - más compacto
            var iconContainer = document.createElement('div');
            iconContainer.classList.add('event-icons');
            iconContainer.style.display = 'flex';
            iconContainer.style.flexDirection = 'row'; // Iconos en horizontal
            iconContainer.style.gap = '2px';
            iconContainer.style.marginRight = '3px';

          
            // Iconos de acciones
            var facturacionIcon = document.createElement('i');
            facturacionIcon.classList.add('ri-bill-line');
            facturacionIcon.title = 'Facturación';
            facturacionIcon.style.fontSize = '16px'; // Aumentado de 12px/14px
            facturacionIcon.style.padding = '3px'; // Aumentado de 1px/2px
            facturacionIcon.addEventListener('click', function (e) {
                e.stopPropagation();
                window.location.href = 'Facturacion/Index?id=' + arg.event.id;
            });

            // Configurar el icono de edición
            var editarIcon = document.createElement('i');
            editarIcon.classList.add('ri-edit-2-line');
            editarIcon.title = 'Editar cita';
            editarIcon.style.cursor = 'pointer';
            editarIcon.style.fontSize = '16px';
            editarIcon.style.padding = '3px';
            editarIcon.addEventListener('click', function (e) {
                e.stopPropagation();
                window.location.href = 'Agenda/EditarCitas?id=' + arg.event.id; // Cambia 'Citas' por tu ruta
            });


            iconContainer.appendChild(editarIcon);

            var historiaIcon = document.createElement('i');
            historiaIcon.classList.add('ri-file-user-line');
            historiaIcon.title = 'Historia clínica';
            historiaIcon.style.fontSize = '16px';
            historiaIcon.style.padding = '3px';
            historiaIcon.addEventListener('click', function (e) {
                e.stopPropagation();
                window.location.href = 'HistoriaClinica/Index?id=' + arg.event.id;
            });

            iconContainer.appendChild(facturacionIcon);
            iconContainer.appendChild(editarIcon);
            iconContainer.appendChild(historiaIcon);

            // Contenido del evento - más compacto
            var eventContent = document.createElement('div');
            eventContent.classList.add('event-content');
            eventContent.style.overflow = 'hidden';
            eventContent.style.display = 'flex';
            eventContent.style.flexDirection = 'column';
            eventContent.style.justifyContent = 'center';
            eventContent.style.height = '100%';

            // Título más compacto
            var title = document.createElement('div');
            title.classList.add('event-title');
            title.style.fontWeight = 'bold';
            title.style.whiteSpace = 'nowrap';
            title.style.overflow = 'hidden';
            title.style.textOverflow = 'ellipsis';
            title.style.lineHeight = '1.2'; // Menor altura de línea
            title.textContent = arg.event.title;
            eventContent.appendChild(title);

            // Hora en vistas de semana/día
            if (arg.view.type !== 'dayGridMonth') {
                var time = document.createElement('div');
                time.classList.add('event-time');
                time.style.fontSize = '0.75em'; // Más pequeño
                time.style.color = '#555';
                time.textContent = arg.timeText;
                eventContent.insertBefore(time, title);
            }

            // Observaciones más compactas
            if (observaciones) {
                var obs = document.createElement('div');
                obs.classList.add('event-observations');
                obs.style.fontSize = '15px'; // Más pequeño
                obs.style.color = '#ffff';
                obs.style.marginTop = '1px';
                obs.style.lineHeight = '1.1';
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
/* ****************************************  FACTURACION JAVASCRIPT  ****************************************** */
// Función para cargar datos en el modal de edición
function cargarDatosModal(tratamientoId, codigo, nombreTratamiento, observaciones, unidades, importeUnitario) {
    document.getElementById('editarTratamientoId').value = tratamientoId;
    document.getElementById('editarNombreTratamiento').value = nombreTratamiento;
    document.getElementById('importeUnitario').value = importeUnitario; // Asigna al campo editable
    document.getElementById('editarUnidades').value = unidades;
    document.getElementById('editarObservaciones').value = observaciones || '';

    // Abre el modal
    var modal = new bootstrap.Modal(document.getElementById('editarTratamientoModal'));
    modal.show();
}
// Función para filtrar la tabla de tratamientos
function filtrarTabla(busqueda) {
    const filas = document.querySelectorAll('#tablaTratamientos tbody tr');
    const textoBusqueda = busqueda.toLowerCase();

    filas.forEach(fila => {
        const textoFila = fila.textContent.toLowerCase();
        fila.style.display = textoFila.includes(textoBusqueda) ? '' : 'none';
    });
}

// Función para seleccionar/deseleccionar todos los checkboxes
function toggleTodos(source) {
    const checkboxes = document.querySelectorAll('input[name="tratamientosSeleccionados"]');
    checkboxes.forEach(checkbox => {
        checkbox.checked = source.checked;
    });
}
function filtrarTabla(busqueda) {
    const filas = document.querySelectorAll('#tablaTratamientos tbody tr');
    const textoBusqueda = busqueda.toLowerCase();

    filas.forEach(fila => {
        const textoFila = fila.textContent.toLowerCase();
        fila.style.display = textoFila.includes(textoBusqueda) ? '' : 'none';
    });
}

// Formatea el precio como moneda
function formatearMoneda(input) {
    // Remover símbolos de moneda y convertir a número
    let valor = parseFloat(input.value.replace(/[^\d,]/g, '').replace(',', '.')) || 0;
    // Formatear como moneda
    input.value = valor.toLocaleString('es-ES', {
        style: 'currency',
        currency: 'EUR'
    });
    return valor; // Devuelve el valor numérico
}

// Calcula el total de una fila y el total general
function calcularTotal(elemento) {
    const fila = elemento.closest('tr');
    const unidades = parseFloat(fila.querySelector('.unidades').value) || 0;
    const precioTexto = fila.querySelector('.precio-unitario').value;

    // Convertir precio de vuelta a número
    const precio = parseFloat(precioTexto.replace(/[^\d,]/g, '').replace(',', '.')) || 0;

    // Calcular y formatear total de la fila
    const totalFila = unidades * precio;
    fila.querySelector('.total').textContent = totalFila.toLocaleString('es-ES', {
        style: 'currency',
        currency: 'EUR'
    });

    // Calcular el total general
    actualizarImporteTotal();
}

// Actualiza el importe total sumando todos los tratamientos
function actualizarImporteTotal() {
    let totalGeneral = 0;

    document.querySelectorAll('tbody tr[id^="tratamiento-"]').forEach(fila => {
        const unidades = parseFloat(fila.querySelector('.unidades').value) || 0;
        const precioTexto = fila.querySelector('.precio-unitario').value;
        const precio = parseFloat(precioTexto.replace(/[^\d,]/g, '').replace(',', '.')) || 0;

        totalGeneral += unidades * precio;
    });

    // Actualizar el campo de importe total usando el ID
    const importeTotalInput = document.getElementById('importeTotal');
    if (importeTotalInput) {
        importeTotalInput.value = totalGeneral.toLocaleString('es-ES', {
            style: 'currency',
            currency: 'EUR'
        });
    }
}



/* ****************************************  FACTURACION JAVASCRIPT   ****************************************** */