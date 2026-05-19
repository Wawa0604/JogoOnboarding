using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Referências Locais da Cena")]
    public DialogueController dialogueController;

    private void Awake()
    {
        // Sem DontDestroyOnLoad! 
        // O Manager da cena atual sempre será a referência ativa para os NPCs locais.
        Instance = this;
    }

    public void IniciarConversa(DialogueSequence sequence)
    {
        if (dialogueController != null)
        {
            dialogueController.StartDialogue(sequence);
        }
        else
        {
            Debug.LogError("DialogueController não foi atribuído no DialogueManager desta cena!");
        }
    }
}