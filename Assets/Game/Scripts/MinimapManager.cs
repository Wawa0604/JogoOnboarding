using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Configurações de Animação")]
    [Tooltip("Velocidade com que o ícone viaja pelo mapa")]
    public float velocidadeNavegacao = 500f; 

    private bool estaExpandido = false;
    private bool estaAnimando = false; // Bloqueia cliques extras durante a viagem
    private Mask mascara;

    void Awake()
    {
        Instance = this;
        mascara = molduraJanela.GetComponent<Mask>();
    }

    void Start()
    {
        estaExpandido = false;
        estaAnimando = false;
        EntrarModoPequeno();
    }

    void Update()
    {
        // Se não estiver expandido, o mapa continua a seguir o jogador normalmente (GPS)
        if (!estaExpandido)
        {
            SincronizarVisualGPS();
        }
    }

    // --- CONTROLO DOS PAINÉIS (Não é mais Toggle) ---

    public void AbrirMapaGrande()
    {
        if (estaAnimando) return;
        estaExpandido = true;
        EntrarModoGrande();
    }

    public void FecharMapaGrande()
    {
        if (estaAnimando) return;
        estaExpandido = false;
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

        molduraJanela.sizeDelta = new Vector2(Screen.width - 100, Screen.height - 100);
        molduraJanela.anchoredPosition = Vector2.zero;

        mapaGrande.anchoredPosition = Vector2.zero;
        SincronizarVisualGPS();
    }

    // --- NAVEGAÇÃO E ANIMAÇÃO ---

    /// <summary>
    /// Método principal que os botões de locais vão chamar ao serem clicados.
    /// </summary>
    public void SelecionarDestino(MapLocation localClicado)
    {
        // Segurança: se já estiver a viajar, ignora novos cliques
        if (estaAnimando || localClicado == null) return;

        StartCoroutine(AnimarIconeEViajar(localClicado));
    }

    private IEnumerator AnimarIconeEViajar(MapLocation destino)
    {
        estaAnimando = true;
        
        // Pega a posição exata do botão na UI do mapa
        Vector2 posicaoAlvo = destino.RetornarRectTransform().anchoredPosition;

        // Enquanto o ícone do player não chegar muito perto da posição do botão...
        while (Vector2.Distance(playerIcon.anchoredPosition, posicaoAlvo) > 0.5f)
        {
            // Move o ícone passo a passo em direção ao botão
            playerIcon.anchoredPosition = Vector2.MoveTowards(
                playerIcon.anchoredPosition, 
                posicaoAlvo, 
                velocidadeNavegacao * Time.deltaTime
            );

            // Espera o próximo frame para continuar a mover suavemente
            yield return null;
        }

        // Garante que ele crava na posição exata no fim
        playerIcon.anchoredPosition = posicaoAlvo;

        // Salva a nova posição no teu configurador para o GPS saber onde o player parou
        if (SceneConfigurator.Instance != null)
        {
            SceneConfigurator.Instance.ultimaPosicaoSalva = posicaoAlvo;
        }

        // Aguarda um mini instante na posição final para dar efeito de chegada
        yield return new WaitForSeconds(0.2f);

        // Carrega a cena do destino
        SceneManager.LoadScene(destino.nomeDaCena);
    }

    void SincronizarVisualGPS()
    {
        if (SceneConfigurator.Instance == null) return;

        Vector2 posReal = SceneConfigurator.Instance.ultimaPosicaoSalva;

        if (!estaExpandido)
        {
            playerIcon.anchoredPosition = Vector2.zero;
            mapaGrande.anchoredPosition = -posReal;
        }
        else if (!estaAnimando)
        {
            // Só sincroniza automaticamente se não estiver no meio da animação de viagem
            playerIcon.anchoredPosition = posReal;
        }
    }
}