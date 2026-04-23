using UnityEngine;

public class DialogoColaboracao : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogue;
    private DialogueController controller;

    void Start()
    {
        // A forma mais atualizada e performática segundo a Unity
        controller = Object.FindAnyObjectByType<DialogueController>();
    }

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
        if (controller != null && dialogue != null)
        {
            controller.StartDialogue(dialogue);
        }
    }
}