using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private InteractionUI ui;

    private DialogueSequence sequence;
    private int index;

    // 🔹 Inicia um diálogo (recebe o asset)
    public void StartDialogue(DialogueSequence newSequence)
    {
        sequence = newSequence;
        index = 0;

        ui.Show();
        UpdateUI();
    }

    // 🔹 Próxima fala
    private void Next()
    {
        if (index < sequence.lines.Length - 1)
        {
            index++;
            UpdateUI();
        }
    }

    // 🔹 Fala anterior
    private void Previous()
    {
        if (index > 0)
        {
            index--;
            UpdateUI();
        }
    }

    // 🔹 Atualiza UI
    private void UpdateUI()
    {
        DialogueLine line = sequence.lines[index];

        ui.SetDialogue(line.characterName, line.text);

        bool hasPrevious = index > 0;
        bool hasNext = index < sequence.lines.Length - 1;

        ui.SetButtonState(hasPrevious, hasNext);
        ui.SetCallbacks(Next, Previous);
    }

    public void EndDialogue()
    {
        ui.Hide();
    }
}