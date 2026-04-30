using UnityEngine;
using UnityEngine.Events;

// Esta classe PRECISA estar aqui ou em um arquivo próprio chamado DialogueLine.cs
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

    [Header("Conteúdo")]
    public DialogueLine[] lines;

    [Header("Progresso de Missão")]
    [Tooltip("Missão concluída AUTOMATICAMENTE ao fechar o diálogo.")]
    public MissaoData missaoParaConcluir;

    [Header("Eventos Extras")]
    [Tooltip("Use para ligar setas ou disparar lógicas customizadas.")]
    public UnityEvent OnSequenceComplete; 
}