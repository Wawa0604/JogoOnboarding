using UnityEngine;

// Representa UMA fala
[System.Serializable]
public class DialogueLine
{
    public string characterName;

    [TextArea]
    public string text;
}

// Representa UMA sequência de diálogo
[CreateAssetMenu(menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueLine[] lines;

    [Header("Progresso de Missão")]
    [Tooltip("Arraste aqui a missão que deve ser concluída ao terminar este diálogo.")]
    public MissaoData missaoParaConcluir;
}