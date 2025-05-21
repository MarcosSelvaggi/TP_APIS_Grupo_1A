function soloNumeros(e) {
    var key = e.keyCode || e.which;
    return (key >= 48 && key <= 57) || key === 8;
}

function validarCP(input) {
    const mensaje = document.getElementById("cpErrorMsj");
    const valor = input.value;

    if (!/^\d*$/.test(valor)) {
        mensaje.textContent = "Solo se permiten números.";
    } else if (valor.length >= 9) {
        mensaje.textContent = "Haz alcanzado el límite de 9 dígitos.";
    } else {
        mensaje.textContent = "";
    }
}

function validarLongitud(input, maxLength, mensajeId) {
    const mensaje = document.getElementById(mensajeId);
    const caracteresDisponibles = maxLength - input.value.length;

    if (caracteresDisponibles <= 5 && caracteresDisponibles > 0) {
        mensaje.textContent = `Te quedan ${caracteresDisponibles} caracteres.`;
    } else if (input.value.length === maxLength) {
        mensaje.textContent = `Has alcanzado el límite de ${maxLength} caracteres.`;
    } else {
        mensaje.textContent = "";
    }
}