using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class QuizDragElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI campoTexto;
    [SerializeField] private Image campoImagem;
    
    public DragItemData Dados { get; private set; }
    
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 posicaoOriginal;
    private Transform paiOriginal;

    public void Configurar(DragItemData dados, Canvas canvasPrincipal)
    {
        Dados = dados;
        canvas = canvasPrincipal;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        paiOriginal = transform.parent;
        posicaoOriginal = rectTransform.anchoredPosition;

        if (campoTexto != null) campoTexto.text = dados.descricao;
        if (campoImagem != null) campoImagem.sprite = dados.spriteObjeto;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Deixa o objeto meio transparente enquanto arrasta e ignora raycasts 
        // para que o slot por trás consiga detectar que soltamos algo nele
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
        // Joga para frente de tudo na UI durante o arrasto
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move o objeto acompanhando o mouse proporcionalmente à escala do Canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Se não foi pego por nenhuma DropZone válida, volta para a estaca zero
        if (transform.parent == paiOriginal)
        {
            rectTransform.anchoredPosition = posicaoOriginal;
        }
    }

    // Efeito visual pedido: Perder alpha e encolher entrando na imagem
    public void ExecutarEfeitoEntrada()
    {
        StartCoroutine(EfeitoEncolherEFade());
    }

    private IEnumerator EfeitoEncolherEFade()
    {
        float duracao = 0.4f;
        float tempo = 0;
        Vector3 escalaOriginal = rectTransform.localScale;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / duracao;

            rectTransform.localScale = Vector3.Lerp(escalaOriginal, Vector3.zero, progresso);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progresso);
            yield return null;
        }

        Destroy(gameObject);
    }
}