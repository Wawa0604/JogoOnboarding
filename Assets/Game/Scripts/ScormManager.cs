 using System.Runtime.InteropServices;
 using UnityEngine;

public class ScormManager : MonoBehaviour

{
    // Importando as funções do .jslib
    [DllImport("__Internal")]
    private static extern void LMSInitialize();
    [DllImport("__Internal")]
    private static extern void LMSSetValue(string key, string value);
    [DllImport("__Internal")]
    private static extern void LMSCommit();

    void Start()
    {
        // Inicializa a comunicação com a Neolude assim que o jogo abre
        #if !UNITY_EDITOR && UNITY_WEBGL
        LMSInitialize();
        #endif
    }

    public void SalvarProgresso(int porcentagemConcluida)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        // No SCORM, "cmi.core.score.raw" é a chave padrão para pontuação
        LMSSetValue("cmi.core.score.raw", porcentagemConcluida.ToString());
        
        // Se chegou no fim, marca como concluído
        if(porcentagemConcluida >= 100)
            LMSSetValue("cmi.core.lesson_status", "completed");

        LMSCommit(); // Garante que a Neolude receba o dado agora
        #endif
    }
}

