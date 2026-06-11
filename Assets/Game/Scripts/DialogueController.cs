using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Header("Controle Físico do Painel (Manual por Cena)")]
    [SerializeField] private GameObject painelDeDialogoManual; 

    [Header("Referências de UI Locais")]
    [SerializeField] private InteractionUI ui;
    
    private DialogueSequence sequence;
    private int index;

    public void StartDialogue(DialogueSequence newSequence)
    {
        if (newSequence == null) return;
        sequence = newSequence;
        index = 0;

        if (painelDeDialogoManual != null)
        {
            painelDeDialogoManual.SetActive(true);
        }

        if (ui != null) 
        {
            ui.Show();
        }
        
        UpdateUI();
    }

    public void Next()
    { 
        if (sequence == null) return;

        if (index < sequence.lines.Length - 1)
        {
            index++;
            UpdateUI();
        }
        else
        {
            EndDialogue();
        }
    }

    public void Previous()
    {
        if (index > 0)
        {
            index--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (ui == null || sequence == null) return;
        
        DialogueLine line = sequence.lines[index];
        ui.SetDialogue(line.characterName, line.text, sequence.iconeAvatar);

        bool hasPrevious = index > 0; 
        bool hasNext = true;         

        ui.SetButtonState(hasPrevious, hasNext);
    }

    public void EndDialogue()
    {
        Debug.Log("[RASTREIO 1] Método EndDialogue foi executado!");

        if (ui != null) ui.Hide();

        if (painelDeDialogoManual != null)
        {
            painelDeDialogoManual.SetActive(false);
        }

        if (sequence != null)
        {
            GameEvents.OnDialogueEnded?.Invoke(sequence.id); 
            sequence.OnSequenceComplete?.Invoke();

            if (sequence.missaoParaConcluir != null && MissionManager.Instance != null)
            {
                MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
            }

            if (sequence.quizParaIniciar != null)
            {
                GameEvents.OnQuizRequested?.Invoke(sequence.quizParaIniciar);
            }

            // DETETIVE DO DRAG QUIZ:
            Debug.Log($"[RASTREIO 2] Checando slot de Quiz Drag. Ele está nulo? {(sequence.quizDragParaIniciar == null ? "SIM" : "NÃO")}");

            if (sequence.quizDragParaIniciar != null)
            {
                Debug.Log($"[RASTREIO 3] Disparando evento no rádio para o quiz: {sequence.quizDragParaIniciar.id}");
                GameEvents.OnDragQuizRequested?.Invoke(sequence.quizDragParaIniciar);
            }
        }
        else
        {
            Debug.LogWarning("[RASTREIO] A sequência de diálogo é nula!");
        }
    }
}