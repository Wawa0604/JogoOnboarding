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
        if (ui != null) ui.Hide();

        if (painelDeDialogoManual != null)
        {
            painelDeDialogoManual.SetActive(false);
        }

        if (sequence != null)
        {
            // Transmissão de eventos de áudio
            GameEvents.OnDialogueEnded?.Invoke(sequence.id); 

            // Evento customizado do Inspector
            sequence.OnSequenceComplete?.Invoke();

            // Sistema de Missões (Ainda usa Singleton por ser um gerenciador global persistente)
            if (sequence.missaoParaConcluir != null && MissionManager.Instance != null)
            {
                MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
            }

            // CORREÇÃO: Transmite o quiz pelo rádio em vez de caçar o Singleton na marra
            if (sequence.quizParaIniciar != null)
            {
                GameEvents.OnQuizRequested?.Invoke(sequence.quizParaIniciar);
            }
        }
    }
}