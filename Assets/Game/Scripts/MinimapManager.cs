using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager Instance;

    [Header("Componentes de UI")]
    public RectTransform playerIcon;
    public RectTransform mapaGrande;
    public RectTransform molduraJanela;
    public GameObject fundoEscuro;

    [Header("Configurações de UI")]
    public Vector2 tamanhoPequeno = new Vector2(350, 250);
    public Vector2 posicaoPequeno = new Vector2(-34, 11);

    [Header("Navegação")]
    public MapLocation[] locaisNoMapa; // Arraste os ícones de locais para cá
    public GameObject avisoInteracao; // Opcional: um texto dizendo "Aperte Espaço"

    private bool estaExpandido = false;
    private Mask mascara;

    void Awake()
    {
        mascara = molduraJanela.GetComponent<Mask>();
    }

    void Start()
    {
        // Inicializa o estado visual
        estaExpandido = false;
        EntrarModoPequeno();
    }

    void Update()
    {
        if (estaExpandido)
        {
            MoverNoModoGrande();
            VerificarProximidadeDeDestinos(); // Nova função!
        }
        else
        {
            SincronizarVisualGPS();
        }
    }

    void VerificarProximidadeDeDestinos()
    {
        bool pertoDeAlguem = false;

        foreach (MapLocation local in locaisNoMapa)
        {
            // Calcula a distância entre o ícone do player e o ícone do local
            float distancia = Vector2.Distance(playerIcon.anchoredPosition, local.GetComponent<RectTransform>().anchoredPosition);

            if (distancia <= local.raioDeAtivacao)
            {
                pertoDeAlguem = true;
                
                // Se o player apertar espaço enquanto estiver perto
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CarregarNovaCena(local.nomeDaCena);
                }
            }
        }

        // Liga/Desliga o aviso visual (se você tiver um)
        if (avisoInteracao != null) avisoInteracao.SetActive(pertoDeAlguem);
    }

    void CarregarNovaCena(string nomeCena)
    {
        Debug.Log("Viajando para: " + nomeCena);
        // O MinimapManager e o SceneConfigurator (Relay) sobrevivem à troca
        SceneManager.LoadScene(nomeCena);
    }

    // Função que o Botão chama
    public void BotaoTrocarTamanho()
    {
        estaExpandido = !estaExpandido;

        if (estaExpandido)
            EntrarModoGrande();
        else
            EntrarModoPequeno();
    }

    void EntrarModoPequeno()
    {
        fundoEscuro.SetActive(false);
        if (mascara != null) mascara.enabled = true;

        molduraJanela.sizeDelta = tamanhoPequeno;
        molduraJanela.anchoredPosition = posicaoPequeno;
        
        SincronizarVisualGPS();
    }

    void EntrarModoGrande()
    {
        fundoEscuro.SetActive(true);
        if (mascara != null) mascara.enabled = false;

        // Ocupa quase a tela toda
        molduraJanela.sizeDelta = new Vector2(Screen.width - 100, Screen.height - 100);
        molduraJanela.anchoredPosition = Vector2.zero;

        // Mapa fica centralizado na moldura
        mapaGrande.anchoredPosition = Vector2.zero;
        SincronizarVisualGPS();
    }

    void MoverNoModoGrande()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        if (h != 0 || v != 0)
        {
            Vector2 movimento = new Vector2(h, v) * 300f * Time.deltaTime;
            
            if (SceneConfigurator.Instance != null)
            {
                SceneConfigurator.Instance.ultimaPosicaoSalva += movimento;
                playerIcon.anchoredPosition = SceneConfigurator.Instance.ultimaPosicaoSalva;
            }
        }
    }

    void SincronizarVisualGPS()
    {
        if (SceneConfigurator.Instance == null) return;

        Vector2 posReal = SceneConfigurator.Instance.ultimaPosicaoSalva;

        if (!estaExpandido)
        {
            // Ícone fica no centro da moldura, mapa desliza atrás
            playerIcon.anchoredPosition = Vector2.zero;
            mapaGrande.anchoredPosition = -posReal;
        }
        else
        {
            // Ícone vai para onde deve estar no mapa
            playerIcon.anchoredPosition = posReal;
        }
    }

    public void SetPlayerPosition(Vector2 novaPos)
    {
        if (SceneConfigurator.Instance != null)
            SceneConfigurator.Instance.ultimaPosicaoSalva = novaPos;
        
        SincronizarVisualGPS();
    }
}