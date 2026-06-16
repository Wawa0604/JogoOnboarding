using UnityEngine;

public enum TipoEtapaQuiz { MultiplaEscolha, ArrastarESoltar }

[System.Serializable]
public class DragItemData
{
    public string idItem; 
    public string descricao;
    public Sprite spriteObjeto;
    public string idTargetCorreto; 
}

[System.Serializable]
public class QuizAlternative
{
    [Header("Conteúdo da Alternativa")]
    public string textoAlternative;
    public Sprite spriteAlternativa; 

    [Header("Validação")]
    public bool ehCorreta;
    
    [TextArea(2, 5)] public string justificativa; 
}

[System.Serializable]
public class QuizQuestion
{
    [Header("Configuração da Etapa")]
    public TipoEtapaQuiz tipoDaEtapa;
    
    [TextArea(3, 5)] 
    public string textoPergunta; // Serve como a pergunta do texto OU o título da rodada de arrastar!

    [Header("Se for Múltipla Escolha:")]
    public QuizAlternative[] alternativas;

    [Header("Se for Arrastar e Soltar:")]
    public DragItemData[] itensParaArrastar;
}

[CreateAssetMenu(fileName = "NovoQuizUnificado", menuName = "Quiz/Sequencia de Quiz Unificado")]
public class QuizSequence : ScriptableObject
{
    public string id; 
    public QuizQuestion[] perguntas; // Array de etapas mistas!
}