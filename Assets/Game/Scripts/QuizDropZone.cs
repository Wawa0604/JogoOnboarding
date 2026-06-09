using UnityEngine;
using UnityEngine.EventSystems;

public class QuizDropZone : MonoBehaviour, IDropHandler
{
    [Tooltip("ID que identifica este slot. Ex: slot_ti, slot_rh")]
    [SerializeField] private string idDestaZona;
    [SerializeField] private QuizDragManager gerenciador;

    public void OnDrop(PointerEventData eventData)
    {
        // Verifica se o objeto que soltamos tem o componente de arrastar
        if (eventData.pointerDrag != null)
        {
            QuizDragElement elementoArrastado = eventData.pointerDrag.GetComponent<QuizDragElement>();
            
            if (elementoArrastado != null)
            {
                // Gruda visualmente o elemento no slot temporariamente para o efeito rodar
                elementoArrastado.transform.SetParent(transform);
                elementoArrastado.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                // Envia a validação para o Manager
                bool ehCorreto = (elementoArrastado.Dados.idTargetCorreto == idDestaZona);
                gerenciador.ProcessarDrop(elementoArrastado, ehCorreto);
            }
        }
    }
}