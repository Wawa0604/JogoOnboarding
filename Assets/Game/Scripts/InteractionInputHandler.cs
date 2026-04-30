using UnityEngine;

public class InteractionInputHandler : MonoBehaviour
{
    // Referência ao controlador de lógica
    [SerializeField] private DialogueController controller;

    // Métodos públicos para você referenciar no OnClick() do Inspector
    
    public void OnClickNext()
    {
        if (controller != null)
        {
            // Chamamos o método do controlador. 
            // Nota: No passo abaixo, vamos precisar mudar o 'Next' do controlador para public.
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