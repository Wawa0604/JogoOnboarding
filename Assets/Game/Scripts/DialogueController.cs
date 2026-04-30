using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private InteractionUI ui;
    private DialogueSequence sequence;
    private int index;

    public void StartDialogue(DialogueSequence newSequence)
    {
        if (newSequence == null) return;
        sequence = newSequence;
        index = 0;

        if (ui != null) ui.Show();
        UpdateUI();
    }

    // MUDANÇA: Agora é PUBLIC para o botão do Inspector acessar
    public void Next()
{ 
    if (sequence == null) {
        Debug.LogError("Nenhuma sequência de diálogo atribuída!");
        return;
    }

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

    // MUDANÇA: Agora é PUBLIC para o botão do Inspector acessar
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
        ui.SetDialogue(line.characterName, line.text);

        // Lógica de botões:
        bool hasPrevious = index > 0; 
        bool hasNext = true;         

        ui.SetButtonState(hasPrevious, hasNext);

    }

public void EndDialogue()
{
    if (ui != null) ui.Hide();

    if (sequence != null)
    {
        // 1. O GRITO NO RÁDIO: Passa o ID para quem estiver ouvindo
        GameEvents.OnDialogueEnded?.Invoke(sequence.id); 

        // 2. O EVENTO DO INSPECTOR: Roda se houver algo configurado
        sequence.OnSequenceComplete?.Invoke();

        // 3. A MISSÃO: Se tiver uma missão no slot, conclui direto
        if (sequence.missaoParaConcluir != null && MissionManager.Instance != null)
        {
            MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
        }
    }
}
}