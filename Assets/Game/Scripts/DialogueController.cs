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
    Debug.Log(">>> O SINAL CHEGOU NO SCRIPT! <<<");
    Debug.Log("Botão Próximo clicado!"); // ADICIONE ESTA LINHA
    
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

        // A LINHA QUE ESTAVA DANDO ERRO (SetCallbacks) FOI REMOVIDA DAQUI!
    }

    public void EndDialogue()
    {
        if (ui != null) ui.Hide();

        // Concluir Missão (Lógica que implementamos antes)
        if (sequence != null && sequence.missaoParaConcluir != null)
        {
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
            }
        }

        if (Game_Manager.Instance != null)
        {
            Game_Manager.Instance.RegistrarFimDeDialogo();
        }
    }
}