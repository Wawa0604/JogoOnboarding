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

    private void Next()
    {
        // Se ainda não chegou na última linha, avança
        if (index < sequence.lines.Length - 1)
        {
            index++;
            UpdateUI();
        }
        else
        {
            // Se já estava na última linha e apertou "Próximo", fecha tudo
            EndDialogue();
        }
    }

    private void Previous()
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
        bool hasPrevious = index > 0; // Desativa "Anterior" se for a primeira frase
        bool hasNext = true;         // SEMPRE ativo para permitir fechar no último clique

        ui.SetButtonState(hasPrevious, hasNext);
        ui.SetCallbacks(Next, Previous);
    }

    public void EndDialogue()
    {
        if (ui != null) ui.Hide();

        // --- ATUALIZAÇÃO PARA REGISTRO DE PROGRESSO ---
        // Sempre que o diálogo fechar, avisamos o gerente global
        if (Game_Manager.Instance != null)
        {
            Debug.Log("DialogueController: Notificando fim de diálogo para o Game_Manager.");
            Game_Manager.Instance.RegistrarFimDeDialogo();
        }
        else
        {
            Debug.LogWarning("DialogueController: Game_Manager.Instance não encontrada ao finalizar diálogo.");
        }
    }
}