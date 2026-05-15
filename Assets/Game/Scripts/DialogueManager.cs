using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // Arraste o componente DialogueController para cá no Inspector
    public DialogueController dialogueController;

    private void Awake()
    {
        // Sem DontDestroyOnLoad! 
        // Toda vez que a cena carregar, este Manager vira a referência principal.
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
            Debug.LogError("DialogueController não atribuído no DialogueManager!");
        }
    }
}