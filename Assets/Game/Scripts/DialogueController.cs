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
            // Transmissão de eventos de áudio / fim de diálogo
            GameEvents.OnDialogueEnded?.Invoke(sequence.id); 
            sequence.OnSequenceComplete?.Invoke();

            // Sistema de Missões
            if (sequence.missaoParaConcluir != null && MissionManager.Instance != null)
            {
                MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
            }

            // O GRANDE TRUNFO UNIFICADO: 
            // Dispara o sinal pelo rádio. O novo QuizManager vai ler o arquivo,
            // descobrir qual é o tipo da primeira etapa e abrir a aba certa sozinho!
            if (sequence.quizParaIniciar != null)
            {
                Debug.Log($"[SISTEMA UNIFICADO] Diálogo concluído. Iniciando a sequência de quiz: {sequence.quizParaIniciar.id}");
                GameEvents.OnQuizRequested?.Invoke(sequence.quizParaIniciar);
            }
        }
    }
}