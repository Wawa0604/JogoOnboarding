using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MapPanner : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("O RectTransform da imagem do avatar")]
    public RectTransform avatarIcon; 
    
    [Header("Configurações de Movimentação")]
    public float velocidadePan = 400f;
    [Tooltip("Distância em pixels da borda da tela para começar a mover")]
    public float margemBorda = 50f; 

    private RectTransform rectMapa;
    private RectTransform rectCanvas;
    
    private Vector2 limiteMin;
    private Vector2 limiteMax;

    private void Awake()
    {
        rectMapa = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            rectCanvas = canvas.GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        CalcularLimites();
        FocarNoAvatar();
    }

    private void CalcularLimites()
    {
        // Calcula quanto o mapa pode se mover sem sair da tela
        if (rectCanvas == null) return;

        float limiteX = Mathf.Max(0, (rectMapa.rect.width - rectCanvas.rect.width) / 2f);
        float limiteY = Mathf.Max(0, (rectMapa.rect.height - rectCanvas.rect.height) / 2f);

        limiteMin = new Vector2(-limiteX, -limiteY);
        limiteMax = new Vector2(limiteX, limiteY);
    }

    private void FocarNoAvatar()
    {
        if (avatarIcon == null) return;

        // Para centralizar o avatar na tela, o mapa deve se mover na direção oposta à posição do avatar
        Vector2 posicaoAlvo = -avatarIcon.anchoredPosition;
        rectMapa.anchoredPosition = AplicarLimites(posicaoAlvo);
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 direcao = Vector3.zero;

        // Verifica se o mouse está nas bordas da tela
        if (mousePos.x < margemBorda) direcao.x = 1; // Move mapa para a direita (revela a esquerda)
        else if (mousePos.x > Screen.width - margemBorda) direcao.x = -1; // Move mapa para a esquerda

        if (mousePos.y < margemBorda) direcao.y = 1; 
        else if (mousePos.y > Screen.height - margemBorda) direcao.y = -1;

        // Aplica o movimento suave se houver direção
        if (direcao != Vector3.zero)
        {
            Vector2 novaPosicao = rectMapa.anchoredPosition + (Vector2)(direcao * velocidadePan * Time.deltaTime);
            rectMapa.anchoredPosition = AplicarLimites(novaPosicao);
        }
    }

    // Impede que o mapa passe da borda da tela
    private Vector2 AplicarLimites(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, limiteMin.x, limiteMax.x);
        pos.y = Mathf.Clamp(pos.y, limiteMin.y, limiteMax.y);
        return pos;
    }
}