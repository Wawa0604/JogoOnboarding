using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DialogoColaboracao : MonoBehaviour
{
    [Header("Referência da Cena")]
    [Tooltip("Arraste o objeto 'UIDocument_Dialogo' que está na sua hierarquia aqui")]
    public GameObject objetoDialogo;

    [Header("Conteúdo dos Diálogos")]
    [TextArea(3, 10)]
    public string[] dialogosIntroducao;

    private int indexAtual = 0;
    private string chaveProgresso;

    // Elementos da UI
    private VisualElement root;
    private Label labelCaixaDialogo;
    private Button botaoProximo;
    private Button botaoAnterior;

    void Awake()
    {
        // Define a chave de progresso
        chaveProgresso = "CenaCompletada_" + SceneManager.GetActiveScene().buildIndex;

        // Se já completou a cena, você pode decidir se quer mostrar de novo ou não
        if (PlayerPrefs.GetInt(chaveProgresso, 0) == 1)
        {
            Debug.Log("Cena já visualizada anteriormente.");
            // Se quiser pular o diálogo se já foi feito:
            // return; 
        }

        PrepararUI();
    }

    void PrepararUI()
    {
        if (objetoDialogo == null) return;

        // Ativa o objeto na cena
        objetoDialogo.SetActive(true);

        // Pega o root do UIDocument
        var uiDoc = objetoDialogo.GetComponent<UIDocument>();
        root = uiDoc.rootVisualElement;

        // BUSCA PELOS NOMES COM # (Conforme sua imagem do UI Builder)
        labelCaixaDialogo = root.Q<Label>("CaixaDialogo");
        botaoProximo = root.Q<Button>("Proximo");
        botaoAnterior = root.Q<Button>("Anterior");

        // Verifica se encontrou (Null Check)
        if (labelCaixaDialogo == null || botaoProximo == null || botaoAnterior == null)
        {
            Debug.LogError("Não encontrei os elementos! Verifique se os nomes no UI Builder são exatamente #CaixaDialogo, #Proximo e #Anterior (sem o # no código se ele for apenas o ID)");
            return;
        }

        // Assina os eventos
        botaoProximo.clicked += ProximoDialogo;
        botaoAnterior.clicked += AnteriorDialogo;

        indexAtual = 0;
        AtualizarInterface();
    }

    void ProximoDialogo()
    {
        if (indexAtual < dialogosIntroducao.Length - 1)
        {
            indexAtual++;
            AtualizarInterface();
        }
        else
        {
            FinalizarDialogo();
        }
    }

    void AnteriorDialogo()
    {
        if (indexAtual > 0)
        {
            indexAtual--;
            AtualizarInterface();
        }
    }

    void AtualizarInterface()
    {
        if (labelCaixaDialogo != null)
            labelCaixaDialogo.text = dialogosIntroducao[indexAtual];

        if (botaoAnterior != null)
            botaoAnterior.SetEnabled(indexAtual > 0);
    }

    void FinalizarDialogo()
    {
        PlayerPrefs.SetInt(chaveProgresso, 1);
        PlayerPrefs.Save();
        
        // Desativa o objeto novamente
        objetoDialogo.SetActive(false);
    }
}