using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("Painéis de UI")]
    [SerializeField] private GameObject painelQuiz;
    [SerializeField] private GameObject painelFimDeJogo;

    [Header("Elementos da Pergunta")]
    [SerializeField] private TextMeshProUGUI textoPerguntaUI;
    [SerializeField] private Transform containerAlternativas;
    [SerializeField] private GameObject prefabBotaoAlternativa;

    [Header("Botão de Ação Central")]
    [SerializeField] private Button btnConfirmar;
    [SerializeField] private TextMeshProUGUI txtBtnConfirmar; 

    [Header("Botões de Fim de Jogo")]
    [SerializeField] private GameObject botaoIrNovamente;
    [SerializeField] private GameObject botaoFechar;

    private QuizSequence quizAtual;
    private int indicePerguntaAtual;
    private bool errouAlguma; 

    public bool ExibindoJustificativa { get; private set; }
    
    private List<QuizAlternativeUI> alternativasNaTela = new List<QuizAlternativeUI>();

    private void OnEnable()
    {
        // Conecta ao rádio de eventos esperando uma chamada de quiz
        GameEvents.OnQuizRequested += IniciarQuiz;
    }

    private void OnDisable()
    {
        // Desconecta ao desativar o objeto para evitar vazamentos de memória
        GameEvents.OnQuizRequested -= IniciarQuiz;
    }

    public void IniciarQuiz(QuizSequence novoQuiz)
    {
        // PASSO 1: O rádio funcionou?
        Debug.Log($"[QUIZ] 1. O rádio funcionou! Quiz recebido: {(novoQuiz != null ? novoQuiz.id : "NULO")}");

        if (novoQuiz == null) {
            Debug.LogError("[QUIZ] ERRO: O arquivo de Quiz enviado pelo diálogo está NULO!");
            return;
        }
        if (novoQuiz.perguntas == null || novoQuiz.perguntas.Length == 0) {
            Debug.LogError("[QUIZ] ERRO: O seu arquivo de Quiz não tem NENHUMA pergunta cadastrada no Inspector!");
            return;
        }

        quizAtual = novoQuiz;
        indicePerguntaAtual = 0;
        errouAlguma = false;

        painelQuiz.SetActive(true);
        painelFimDeJogo.SetActive(false);

        btnConfirmar.onClick.RemoveAllListeners();
        btnConfirmar.onClick.AddListener(OnBotaoAcaoPrincipalClick);

        ExibirPergunta();
    }

    private void ExibirPergunta()
    {
        ExibindoJustificativa = false;
        txtBtnConfirmar.text = "Confirmar";
        btnConfirmar.interactable = false; 

        // Limpa alternativas anteriores
        foreach (Transform child in containerAlternativas) Destroy(child.gameObject);
        alternativasNaTela.Clear();

        QuizQuestion pergunta = quizAtual.perguntas[indicePerguntaAtual];
        textoPerguntaUI.text = pergunta.textoPergunta;

        // PASSO 2: Quantas alternativas existem?
        Debug.Log($"[QUIZ] 2. Exibindo pergunta: '{pergunta.textoPergunta}'. Total de alternativas encontradas: {pergunta.alternativas.Length}");

        if (pergunta.alternativas == null || pergunta.alternativas.Length == 0) {
            Debug.LogWarning("[QUIZ] Aviso: Essa pergunta específica não tem nenhuma alternativa criada!");
        }

        // Cria os botões das alternativas
        foreach (QuizAlternative alt in pergunta.alternativas)
        {
            // PASSO 3: Tentando instanciar
            Debug.Log($"[QUIZ] 3. Instanciando botão para a alternativa: '{alt.textoAlternativa}'");

            GameObject go = Instantiate(prefabBotaoAlternativa, containerAlternativas);
            
            // Ela força o clone a ligar na marra, independente de onde veio o prefab
           // go.SetActive(true);

            //Debug.Log($"[DETETIVE] Botão: {go.name} | Local Ativo (Caixinha): {go.activeSelf} | Ativo na Cena: {go.activeInHierarchy}");

            QuizAlternativeUI scriptAlt = go.GetComponent<QuizAlternativeUI>();
            
            // PASSO 4: O Prefab está correto?
            if (scriptAlt == null) {
                Debug.LogError($"[QUIZ] ERRO CRÍTICO: O seu prefab de botão não possui o script 'QuizAlternativeUI' anexado a ele! A criação parou aqui.");
                continue;
            }

            scriptAlt.Configurar(alt, this);
            alternativasNaTela.Add(scriptAlt);
        }

        // PASSO 5: Finalização
        Debug.Log($"[QUIZ] 4. Renderização finalizada. Botões na tela: {alternativasNaTela.Count}");
    }

    public void AtualizarBotaoConfirmar()
    {
        if (ExibindoJustificativa) return;

        btnConfirmar.interactable = alternativasNaTela.Exists(x => x.IsSelected);
    }

    private void OnBotaoAcaoPrincipalClick()
    {
        if (!ExibindoJustificativa)
        {
            MostrarJustificativasEResultados();
        }
        else
        {
            AvançarParaProxima();
        }
    }

    private void MostrarJustificativasEResultados()
    {
        ExibindoJustificativa = true;
        txtBtnConfirmar.text = "Avançar"; 

        bool acertouTudoNessaQuestao = true;

        foreach (QuizAlternativeUI altUI in alternativasNaTela)
        {
            altUI.RevelarResultado(); 

            if (altUI.Dados.ehCorreta && !altUI.IsSelected) acertouTudoNessaQuestao = false;
            if (!altUI.Dados.ehCorreta && altUI.IsSelected) acertouTudoNessaQuestao = false;
        }

        if (!acertouTudoNessaQuestao)
        {
            errouAlguma = true; 
        }
    }

    private void AvançarParaProxima()
    {
        indicePerguntaAtual++;
        
        // LOG DETETIVE 1: Vendo se o contador está subindo corretamente
        Debug.Log($"[FIM DE JOGO] Avançando. Índice Atual: {indicePerguntaAtual} | Total de Perguntas: {quizAtual.perguntas.Length}");

        if (indicePerguntaAtual < quizAtual.perguntas.Length)
        {
            ExibirPergunta();
        }
        else
        {
            // LOG DETETIVE 2: Entrou na condição de término?
            Debug.Log("[FIM DE JOGO] Todas as perguntas foram respondidas! Chamando FinalizarRodada().");
            FinalizarRodada();
        }
    }

    private void FinalizarRodada()
    {
        Debug.Log("[FIM DE JOGO] Iniciou a execução do método FinalizarRodada().");

        if (painelQuiz != null) painelQuiz.SetActive(false);
        
        // VALIDAÇÃO CRÍTICA: O painel existe no Inspector?
        if (painelFimDeJogo != null)
        {
            painelFimDeJogo.SetActive(true);
            Debug.Log("[FIM DE JOGO] Comando painelFimDeJogo.SetActive(true) executado com sucesso!");
        }
        else
        {
            Debug.LogError("[FIM DE JOGO] ERRO CRÍTICO: O slot 'Painel Fim De Jogo' está VAZIO no seu QuizManager no Inspector!");
        }

        // Proteções contra falta de atribuição dos botões finais
        if (botaoIrNovamente != null) 
        {
            botaoIrNovamente.SetActive(true);
        }
        else 
        {
            Debug.LogWarning("[FIM DE JOGO] Aviso: O slot 'Botao Ir Novamente' está vazio.");
        }

        if (botaoFechar != null) 
        {
            botaoFechar.SetActive(!errouAlguma);
        }
        else 
        {
            Debug.LogWarning("[FIM DE JOGO] Aviso: O slot 'Botao Fechar' está vazio.");
        }
    }
    public void ReiniciarQuiz() => IniciarQuiz(quizAtual);
    public void FecharQuiz() => painelFimDeJogo.SetActive(false);
}