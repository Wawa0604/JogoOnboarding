using UnityEngine;

public class DialogoColaboracao : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogue;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
    {
        Debug.Log("Tecla E pressionada!"); // Adicione isso
        Interact();
    }
        // Interação por tecla
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        // 1. Corrigido para usar o nome exato da classe: Game_Manager
        if (Game_Manager.Instance != null)
        {
            // 2. Corrigido também aqui para manter o padrão
            DialogueController controller = Game_Manager.Instance.GetComponent<DialogueController>();

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
            // Se o erro persistir, é porque o Game_Manager ainda não deu Awake()
            Debug.LogWarning("Game_Manager.Instance ainda não está pronta ou não existe na cena.");
        }
    }
}