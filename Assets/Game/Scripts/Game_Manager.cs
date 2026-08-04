using UnityEngine;
using System.Collections.Generic; // Necessário para os Dicionários

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    [Header("Dados de Navegação")]
    public Vector2 ultimaPosicaoSalva = Vector2.zero;

    // ==========================================
    // NOVO: Memória do Avatar para persistir entre as cenas
    // ==========================================
    public Dictionary<string, int> avatarParts = new Dictionary<string, int>();
    public Dictionary<string, Color> avatarColors = new Dictionary<string, Color>();

    private MissionDataManager missionDataManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            missionDataManager = GetComponent<MissionDataManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnMapPositionSaved += SalvarPosicaoMapa;
        
        // Escuta o evento de salvar o avatar
        GameEvents.OnAvatarSaved += SalvarDadosDoAvatar; 
    }

    private void OnDisable()
    {
        GameEvents.OnMapPositionSaved -= SalvarPosicaoMapa;
        GameEvents.OnAvatarSaved -= SalvarDadosDoAvatar;
    }

    // Guarda os dados enviados pelo painel de customização
    private void SalvarDadosDoAvatar(Dictionary<string, int> parts, Dictionary<string, Color> colors)
    {
        avatarParts = new Dictionary<string, int>(parts);
        avatarColors = new Dictionary<string, Color>(colors);
        Debug.Log("Game_Manager: Dados do Avatar guardados com sucesso para as próximas cenas!");
    }

    private void SalvarPosicaoMapa(Vector2 novaPosicao)
    {
        ultimaPosicaoSalva = novaPosicao;
    }


    public void RegistrarFimDeDialogo()
    {
        Debug.Log("Game_Manager: Processando fim de diálogo...");
        int conversasLidas = PlayerPrefs.GetInt("ConversasFinalizadas", 0);
        conversasLidas++;
        PlayerPrefs.SetInt("ConversasFinalizadas", conversasLidas);
        PlayerPrefs.Save();

        if (missionDataManager != null)
        {
            // missionDataManager.CheckMissionProgress(); 
        }
    }
}