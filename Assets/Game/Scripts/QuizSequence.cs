using UnityEngine;

[System.Serializable]
public class QuizAlternative
{
    [Header("Conteúdo da Alternativa")]
    public string textoAlternativa;
    public Sprite spriteAlternativa; // Se quiser uma imagem no lugar de texto

    [Header("Validação")]
    public bool ehCorreta;
    
    [TextArea(2, 5)] 
    public string justificativa; // <--- NOVA: O texto explicando por que está errada/certa
}

[System.Serializable]
public class QuizQuestion
{
    [TextArea(3, 5)] public string textoPergunta;
    public QuizAlternative[] alternativas;
}

[CreateAssetMenu(fileName = "NovoQuiz", menuName = "Quiz/Sequencia de Quiz")]
public class QuizSequence : ScriptableObject
{
    public string id; // ID para o rádio, se precisar
    public QuizQuestion[] perguntas;
}