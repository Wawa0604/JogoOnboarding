using UnityEngine;

public class InteractionInputHandler : MonoBehaviour
{
    [Header("Referências Manuais da Cena")]
    [SerializeField] private DialogueController controller;

    public void OnClickNext()
    {
        if (controller != null)
        {
            controller.Next(); 
        }
    }

    public void OnClickPrevious()
    {
        if (controller != null)
        {
            controller.Previous();
        }
    }
}