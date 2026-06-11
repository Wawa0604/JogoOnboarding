using UnityEngine;
using UnityEngine.Events;

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
    [Tooltip("ID único para o rádio. Ex: intro_npc, tutorial_avatar")]
    public string id; 

    [Header("Foto")]
    public Sprite iconeAvatar;

    [Header("Conteúdo")]
    public DialogueLine[] lines;

    [Header("Progresso de Missão")]
    [Tooltip("Missão concluída AUTOMATICAMENTE ao fechar o diálogo.")]
    public MissaoData missaoParaConcluir;

    [Header("Eventos Extras")]
    [Tooltip("Use para ligar setas ou disparar lógicas customizadas.")]
    public UnityEvent OnSequenceComplete; 

    [Header("Conexão com Quizzes")]
    [Tooltip("Arraste aqui o arquivo de Quiz TRADICIONAL que deve iniciar.")]
    public QuizSequence quizParaIniciar; 

    // NOVO SLOT ADICIONADO AQUI:
    [Tooltip("Arraste aqui o arquivo de Quiz de ARRASTAR que deve iniciar.")]
    public QuizDragSequence quizDragParaIniciar; 
}