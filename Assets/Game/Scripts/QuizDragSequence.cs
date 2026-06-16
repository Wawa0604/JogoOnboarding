using UnityEngine;

[System.Serializable]
public class DragItemData
{
    public string idItem; // Ex: "cabo_rede", "mouse"
    public string descricao;
    public Sprite spriteObjeto;
    public string idTargetCorreto; // ID do slot para onde ele deve ir
}

[CreateAssetMenu(fileName = "NovoQuizArrastar", menuName = "Quiz/Sequencia Arrastar")]
public class QuizDragSequence : ScriptableObject
{
    public string id;
    public string pergunta;
    public DragItemData[] itensParaArrastar;
}