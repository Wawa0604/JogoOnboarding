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
    [DllImport("__Internal")]
    private static extern string LMSGetValue(string key);

    void Start()
{
    #if !UNITY_EDITOR && UNITY_WEBGL
    LMSInitialize();

    // Captura o nome do aluno da Neolude para o seu sistema interno
    string aluno = LMSGetValue("cmi.core.student_name"); 
    
    // Se o aluno for nulo ou vazio (comum em alguns LMS), define um padrão
    if (string.IsNullOrEmpty(aluno)) aluno = "Jogador_SCORM";

    if (GameManager.Instance != null)
    {
        GameManager.Instance.SavePlayer(aluno);
    }
    #endif
}


        public string GetStudentName()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        return LMSGetValue("cmi.core.student_name");
        #else
        return "Editor Mode";
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

