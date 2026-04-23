using UnityEngine;

public class DialogoColaboracao : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogue;

    void Update()
    {
        // Interação por tecla
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        // 1. Tenta acessar o GameManager através da Instance estática
        if (GameManager.Instance != null)
        {
            // 2. Busca o DialogueController que está anexado ao GameManager
            DialogueController controller = GameManager.Instance.GetComponent<DialogueController>();

            if (controller != null)
            {
                controller.StartDialogue(dialogue);
            }
            else
            {
                Debug.LogError("DialogueController não foi encontrado no objeto GameManager!");
            }
        }
        else
        {
            // Isso acontece se você der Play direto na cena sem o GameManager
            Debug.LogWarning("GameManager.Instance não encontrada nesta cena.");
        }
    }
}