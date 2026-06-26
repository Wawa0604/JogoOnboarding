using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScormManager : MonoBehaviour
{
    // Importações do JavaScript (LMS SCORM 1.2 padrão da Neolude)
    [DllImport("__Internal")] private static extern void LMSInitialize();
    [DllImport("__Internal")] private static extern void LMSSetValue(string key, string value);
    [DllImport("__Internal")] private static extern void LMSCommit();
    [DllImport("__Internal")] private static extern string LMSGetValue(string key);
 
    public static ScormManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject); 
            return; 
        }
    }

    IEnumerator Start()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        yield return new WaitForSeconds(2.0f); 
        LMSInitialize();

        string aluno = LMSGetValue("cmi.core.student_name"); 
        Debug.Log("SCORM conectado para o aluno: " + aluno);
        
        // Atualiza o progresso inicial (caso ele já tenha carregado missões do PlayerPrefs)
        DispararAtualizacaoLMS();
        #endif
        yield break;
    }

    // --- MÉTODO ACESSÍVEL PELO MISSION MANAGER ---
    public void DispararAtualizacaoLMS()
    {
        if (MissionManager.Instance != null)
        {
            int progresso = MissionManager.Instance.ObterPorcentagemConcluida();
            SalvarNoLMS(progresso);
        }
    }

    private void SalvarNoLMS(int porcentagemTotal)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        // Envia a nota/progresso atual para a Neolude de 0 a 100
        LMSSetValue("cmi.core.score.raw", porcentagemTotal.ToString());
        
        // Define o status da lição baseado nos 100% das missões cumpridas
        if (porcentagemTotal >= 100)
        {
            LMSSetValue("cmi.core.lesson_status", "completed");
        }
        else
        {
            // Opcional para manter o status ativo na plataforma enquanto joga
            LMSSetValue("cmi.core.lesson_status", "incomplete");
        }

        LMSCommit(); 
        Debug.Log($"SCORM Neolude: Progresso de {porcentagemTotal}% enviado.");
        #endif
    }
}