var scormAPI = null;

function findAPI(win) {
    var attempts = 0;
    while ((win.API == null) && (win.parent != null) && (win.parent != win)) {
        attempts++;
        if (attempts > 7) return null;
        win = win.parent;
    }
    return win.API;
}

function initScorm() {
    scormAPI = findAPI(window);
    if (scormAPI) {
        scormAPI.LMSInitialize("");
        console.log("SCORM 1.2 Inicializado com sucesso.");
    } else {
        console.warn("API SCORM não encontrada. Rodando fora da Neolude?");
    }
}

function setScormValue(key, value) {
    if (scormAPI) {
        scormAPI.LMSSetValue(key, value);
    }
}

function getScormValue(key) {
    if (scormAPI) {
        return scormAPI.LMSGetValue(key);
    }
    return "";
}

function commitScorm() {
    if (scormAPI) {
        scormAPI.LMSCommit("");
    }
}

// Inicializa automaticamente quando o HTML carrega
window.onload = initScorm;