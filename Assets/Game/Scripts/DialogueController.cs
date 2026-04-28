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

        // --- CONCLUIR MISSÃO AUTOMATICAMENTE ---
        if (sequence != null && sequence.missaoParaConcluir != null)
        {
            if (MissionManager.Instance != null)
            {
                Debug.Log($"DialogueController: Solicitando conclusão da missão {sequence.missaoParaConcluir.id}");
                MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
            }
            else
            {
                Debug.LogWarning("DialogueController: MissionManager.Instance não encontrado!");
            }
        }

        // --- NOTIFICAR GAME MANAGER ---
        if (Game_Manager.Instance != null)
        {
            Game_Manager.Instance.RegistrarFimDeDialogo();
        }
    }
}