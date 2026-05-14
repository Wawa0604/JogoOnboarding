mergeInto(LibraryManager.library, {
  
  LMSInitialize: function () {
    window.parent.LMSInitialize("");
  },

  LMSSetValue: function (key, value) {
    // UTF8ToString é o padrão atual para converter ponteiros de string do Unity
    window.parent.LMSSetValue(UTF8ToString(key), UTF8ToString(value));
  },

  // ADICIONADO: Função para buscar dados da Neolude (como nome do aluno)
  LMSGetValue: function (key) {
    var returnValue = window.parent.LMSGetValue(UTF8ToString(key));
    var size = lengthBytesUTF8(returnValue) + 1;
    var buffer = _malloc(size);
    stringToUTF8(returnValue, buffer, size);
    return buffer;
  },

  LMSCommit: function () {
    window.parent.LMSCommit("");
  },

  LMSFinish: function () {
    window.parent.LMSFinish("");
  }
});