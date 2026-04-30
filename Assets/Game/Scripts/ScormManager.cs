using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScormManager : MonoBehaviour
{
    // Importações do JavaScript (LMS SCORM 1.2)
    [DllImport("__Internal")] private static extern void LMSInitialize();
    [DllImport("__Internal")] private static extern void LMSSetValue(string key, string value);
    [DllImport("__Internal")] private static extern void LMSCommit();
    [DllImport("__Internal")] private static extern string LMSGetValue(string key);

    public static ScormManager Instance;

    void Awake()
    {
        // Torna o ScormManager o novo "Porto Seguro" persistente
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); return; }
    }

    IEnumerator Start()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        yield return new WaitForSeconds(2.0f); 
        LMSInitialize();

        // O nome do aluno agora fica apenas no log ou uso interno do SCORM
        string aluno = LMSGetValue("cmi.core.student_name"); 
        Debug.Log("SCORM conectado para o aluno: " + aluno);
        #endif
        yield break;
    }

    // --- OUVINTE DE EVENTOS ---

    private void OnEnable()
    {
        // Quando qualquer missão terminar, este método será chamado
        GameEvents.OnMissionCompleted += AtualizarProgressoLMS;
    }

    private void OnDisable()
    {
        GameEvents.OnMissionCompleted -= AtualizarProgressoLMS;
    }

    // --- LÓGICA DE ENVIO ---

    private void AtualizarProgressoLMS(string idMissao)
    {
        // Se você quiser que cada missão envie uma porcentagem específica,
        // você pode calcular aqui ou buscar no MissionManager.
        
        // Exemplo: Se o MissionManager tem o cálculo de porcentagem:
        if (MissionManager.Instance != null)
        {
            int progresso = MissionManager.Instance.ObterPorcentagemConcluida();
            SalvarNoLMS(progresso);
        }
    }

    private void SalvarNoLMS(int porcentagemTotal)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        LMSSetValue("cmi.core.score.raw", porcentagemTotal.ToString());
        
        if(porcentagemTotal >= 100)
            LMSSetValue("cmi.core.lesson_status", "completed");

        LMSCommit(); 
        Debug.Log($"SCORM: Progresso de {porcentagemTotal}% enviado após conclusão de missão.");
        #endif
    }
}