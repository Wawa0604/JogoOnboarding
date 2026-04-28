using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScormManager : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void LMSInitialize();
    [DllImport("__Internal")] private static extern void LMSSetValue(string key, string value);
    [DllImport("__Internal")] private static extern void LMSCommit();
    [DllImport("__Internal")] private static extern string LMSGetValue(string key);

    IEnumerator Start()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        yield return new WaitForSeconds(2.0f); // Tempo para o LMS injetar a API
        LMSInitialize();

        string aluno = LMSGetValue("cmi.core.student_name"); 
        if (string.IsNullOrEmpty(aluno)) aluno = "Jogador_SCORM";

        if (GameManager.Instance != null) GameManager.Instance.playerEmail = aluno;
        #endif
        yield break;
    }

    public void SalvarProgressoFinal(int porcentagemTotal)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        LMSSetValue("cmi.core.score.raw", porcentagemTotal.ToString());
        
        if(porcentagemTotal >= 100)
            LMSSetValue("cmi.core.lesson_status", "completed");

        LMSCommit(); // Commit definitivo ao fim da missão
        Debug.Log($"SCORM: Progresso de {porcentagemTotal}% enviado com sucesso.");
        #endif
    }
}