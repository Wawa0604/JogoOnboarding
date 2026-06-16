using UnityEngine;
using UnityEngine.Events;

// Devolvido aqui para o Unity voltar a reconhecer as linhas de texto!
[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)] public string text;
}

[CreateAssetMenu(menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    [Header("Identificação")]
    public string id; 
    public Sprite iconeAvatar;
    public DialogueLine[] lines;
    public MissaoData missaoParaConcluir;
    public UnityEvent OnSequenceComplete; 

    [Header("Conexão com Quiz")]
    [Tooltip("Arraste aqui o Quiz Unificado (pode conter perguntas normais, de arrastar ou misturadas).")]
    public QuizSequence quizParaIniciar; 
}