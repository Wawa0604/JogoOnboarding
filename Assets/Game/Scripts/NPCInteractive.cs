using UnityEngine;

public class NPCInteractive : MonoBehaviour
{
/// Script genérico para qualquer NPC que inicie um diálogo.
    [Header("Configurações de Diálogo")]
    [SerializeField] private DialogueSequence dialogoParaDisparar;
    
    [Header("Feedback Visual")]
    [SerializeField] private GameObject indicativoVisual; // Ex: A seta que tens na Foto {BA2506A8-7FA6-4EAF-A0B8-F2D373C3BB1C}.png

    void OnMouseDown()
    {
        Interact();
    }

    public void Interact()
    {
        // 1. Esconde a seta ou exclamação sobre o NPC
        if (indicativoVisual != null)
        {
            indicativoVisual.SetActive(false);
        }

        // 2. Tenta falar com o DialogueManager
        if (DialogueManager.Instance != null)
        {
            // Dispara o diálogo configurado no Inspector
            DialogueManager.Instance.IniciarConversa(dialogoParaDisparar);
        }
        else
        {
            Debug.LogWarning("O DialogueManager não foi encontrado nesta cena! Certifica-te que o Prefab está na hierarquia.");
        }
    }
}

