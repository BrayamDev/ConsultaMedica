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

