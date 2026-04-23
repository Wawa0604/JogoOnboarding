mergeInto(LibraryManager.library, {
  
  // Função para avisar o SCORM que o jogo começou
  LMSInitialize: function () {
    window.parent.LMSInitialize("");
  },

  // Função para enviar a nota/progresso (0 a 100)
  LMSSetValue: function (key, value) {
    var keyStr = Pointer_stringify(key);
    var valStr = Pointer_stringify(value);
    window.parent.LMSSetValue(keyStr, valStr);
  },

  // Função para salvar os dados
  LMSCommit: function () {
    window.parent.LMSCommit("");
  },

  // Função para finalizar a sessão
  LMSFinish: function () {
    window.parent.LMSFinish("");
  }
});