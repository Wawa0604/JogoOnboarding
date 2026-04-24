using UnityEngine;

public class DialogoColaboracao : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogue;
    [SerializeField] private GameObject seta; // Arraste a imagem da seta aqui no Inspetor

    void OnMouseDown()
    {
        // Este método é chamado automaticamente pelo Unity quando o objeto é clicado
        Interact();
    }

    public void Interact()
    {
        // Desativa a seta se ela estiver atribuída
        if (seta != null)
        {
            seta.SetActive(false);
        }

        // Lógica do DialogueManager
        if (DialogueManager.Instance != null)
        {
            DialogueController controller = DialogueManager.Instance.GetComponent<DialogueController>();

            if (controller != null)
            {
                controller.StartDialogue(dialogue);
            }
            else
            {
                Debug.LogError("DialogueController não encontrado no objeto DialogueManager!");
            }
        }
        else
        {
            Debug.LogWarning("DialogueManager não encontrado nesta cena!");
        }
    }
}