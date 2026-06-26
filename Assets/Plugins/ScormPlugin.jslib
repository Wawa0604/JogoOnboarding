mergeInto(LibraryManager.library, {

  LMSInitialize: function () {
    if (typeof window.initScorm === 'function') {
        window.initScorm();
    } else {
        console.warn("Aviso: Função initScorm não encontrada no HTML.");
    }
  },

  LMSSetValue: function (key, value) {
    var keyStr = UTF8ToString(key);
    var valueStr = UTF8ToString(value);
    if (typeof window.setScormValue === 'function') {
        window.setScormValue(keyStr, valueStr);
    }
  },

  LMSGetValue: function (key) {
    var keyStr = UTF8ToString(key);
    var result = "";
    
    if (typeof window.getScormValue === 'function') {
        result = window.getScormValue(keyStr);
    }
    
    var bufferSize = lengthBytesUTF8(result) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(result, buffer, bufferSize);
    return buffer;
  },

  LMSCommit: function () {
    if (typeof window.commitScorm === 'function') {
        window.commitScorm();
    }
  }

});