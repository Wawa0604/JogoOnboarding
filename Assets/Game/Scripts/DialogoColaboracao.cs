using UnityEngine;

public class DialogoColaboracao : MonoBehaviour
{
     [SerializeField] private DialogueSequence dialogue;
    private DialogueController controller;

    void Start()
    {
        controller = FindObjectOfType<DialogueController>();
    }

    void Update()
    {
        // TESTE simples (depois você troca por trigger real)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        controller.StartDialogue(dialogue);
    }
}
