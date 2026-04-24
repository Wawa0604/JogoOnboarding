using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    [Header("Dados do Jogador")]
    public string currentPlayerEmail = "usuario_scorm";
    
    // Referência interna para o gerenciador de missões
    private MissionDataManager missionDataManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Tenta encontrar o MissionDataManager no mesmo objeto
            missionDataManager = GetComponent<MissionDataManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayer(string email)
    {
        if (string.IsNullOrEmpty(email)) return;
        
        currentPlayerEmail = email;
        Debug.Log("Jogador definido como: " + email);
        
        // Salva o e-mail no PlayerPrefs para persistência entre sessões
        PlayerPrefs.SetString("PlayerEmail", email);
        PlayerPrefs.Save();
    }

    // Método chamado pelo DialogueController ao terminar uma conversa
    public void RegistrarFimDeDialogo()
    {
        Debug.Log("Game_Manager: Processando fim de diálogo...");

        // 1. Salva o progresso numericamente no PlayerPrefs
        int conversasLidas = PlayerPrefs.GetInt("ConversasFinalizadas", 0);
        conversasLidas++;
        PlayerPrefs.SetInt("ConversasFinalizadas", conversasLidas);
        PlayerPrefs.Save();

        Debug.Log($"Progresso salvo: {conversasLidas} diálogos concluídos.");

        // 2. Avisa o MissionDataManager para atualizar o status das missões
        if (missionDataManager != null)
        {
            // Aqui você chama o método de atualizar missões do seu script
            // missionDataManager.CheckMissionProgress(); 
        }
    }
}