using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("Abas Visuais do Painel")]
    [SerializeField] private GameObject painelGeralQuiz;
    [SerializeField] private GameObject abaMultiplaEscolha;
    [SerializeField] private GameObject abaArrastar;
    [SerializeField] private GameObject painelFimDeJogo;

    [Header("Componentes de Texto Compartilhados")]
    [SerializeField] private TextMeshProUGUI textoPerguntaMultiplaUI;
    [SerializeField] private TextMeshProUGUI textoPerguntaDragUI;

    [Header("Configurações: Múltipla Escolha")]
    [SerializeField] private Transform containerAlternativas;
    [SerializeField] private GameObject prefabBotaoAlternativa;
    [SerializeField] private Button btnConfirmar;
    [SerializeField] private TextMeshProUGUI txtBtnConfirmar; 

    [Header("Configurações: Arrastar e Soltar")]
    [SerializeField] private Canvas canvasPrincipal;
    [SerializeField] private GameObject prefabArrastavel;
    [SerializeField] private Transform localSpawnElemento; 
    [SerializeField] private CanvasGroup canvasGroupFeedbackCorreto;
    [SerializeField] private CanvasGroup canvasGroupFeedbackErrado;

    [Header("Componentes Compartilhados de Fim de Jogo")]
    [SerializeField] private GameObject botaoIrNovamente;
    [SerializeField] private GameObject botaoFechar;

    // Variáveis de Controle de Fluxo
    private QuizSequence quizAtual;
    private int indicePerguntaGlobal; // Qual pergunta do array estamos
    private int indiceDragItemLocal;  // Qual objeto do arraste atual está na tela
    private bool errouAlgumaNoQuizInteiro; 
    private bool exibindoJustificativa;
    
    private List<QuizAlternativeUI> alternativasNaTela = new List<QuizAlternativeUI>();
    public bool ExibindoJustificativa => exibindoJustificativa;

    private void OnEnable()
    {
        GameEvents.OnQuizRequested += IniciarQuizGeral;
        GameEvents.OnRestartDragQuizRequested += ReiniciarQuizAtual; // Escuta o botão jogar novamente
    }

    private void OnDisable()
    {
        GameEvents.OnQuizRequested -= IniciarQuizGeral;
        GameEvents.OnRestartDragQuizRequested -= ReiniciarQuizAtual;
    }

    public void IniciarQuizGeral(QuizSequence novoQuiz)
    {
        if (novoQuiz == null || novoQuiz.perguntas.Length == 0) return;

        quizAtual = novoQuiz;
        indicePerguntaGlobal = 0;
        errouAlgumaNoQuizInteiro = false;

        painelGeralQuiz.SetActive(true);
        if (painelFimDeJogo != null) painelFimDeJogo.SetActive(false);

        DefinirEConfigurarEtapaAtual();
    }

    private void DefinirEConfigurarEtapaAtual()
    {
        // Se passamos do limite do array, o quiz inteiro acabou!
        if (indicePerguntaGlobal >= quizAtual.perguntas.Length)
        {
            FinalizarRodadaGeral();
            return;
        }

        QuizQuestion etapaAtual = quizAtual.perguntas[indicePerguntaGlobal];

        // Reseta os painéis de feedback visual
        if (canvasGroupFeedbackCorreto != null) canvasGroupFeedbackCorreto.alpha = 0f;
        if (canvasGroupFeedbackErrado != null) canvasGroupFeedbackErrado.alpha = 0f;

        if (etapaAtual.tipoDaEtapa == TipoEtapaQuiz.MultiplaEscolha)
        {
            abaMultiplaEscolha.SetActive(true);
            abaArrastar.SetActive(false);
            ConfigurarEtapaMultiplaEscolha(etapaAtual);
        }
        else if (etapaAtual.tipoDaEtapa == TipoEtapaQuiz.ArrastarESoltar)
        {
            abaMultiplaEscolha.SetActive(false);
            abaArrastar.SetActive(true);
            indiceDragItemLocal = 0; // Começa o sub-contador de itens do zero
            ConfigurarEtapaArrastar(etapaAtual);
        }
    }

    // =================================================================
    // PROCESSAMENTO: MÚLTIPLA ESCOLHA
    // =================================================================
    private void ConfigurarEtapaMultiplaEscolha(QuizQuestion pergunta)
    {
        exibindoJustificativa = false;
        txtBtnConfirmar.text = "Confirmar";
        btnConfirmar.interactable = false; 

        foreach (Transform child in containerAlternativas) Destroy(child.gameObject);
        alternativasNaTela.Clear();

        if (textoPerguntaMultiplaUI != null) textoPerguntaMultiplaUI.text = pergunta.textoPergunta;

        btnConfirmar.onClick.RemoveAllListeners();
        btnConfirmar.onClick.AddListener(OnBotaoAcaoPrincipalClick);

        foreach (QuizAlternative alt in pergunta.alternativas)
        {
            GameObject go = Instantiate(prefabBotaoAlternativa, containerAlternativas);
            QuizAlternativeUI scriptAlt = go.GetComponent<QuizAlternativeUI>();
            if (scriptAlt != null)
            {
                scriptAlt.Configurar(alt, this);
                alternativasNaTela.Add(scriptAlt);
            }
        }
    }

    public void AtivarBotaoConfirmar()
    {
        if (exibindoJustificativa) return;
        btnConfirmar.interactable = alternativasNaTela.Exists(x => x.IsSelected);
    }

    private void OnBotaoAcaoPrincipalClick()
    {
        if (!exibindoJustificativa)
        {
            exibindoJustificativa = true;
            txtBtnConfirmar.text = "Avançar"; 
            bool acertouTudoNessaQuestao = true;

            foreach (QuizAlternativeUI altUI in alternativasNaTela)
            {
                altUI.RevelarResultado(); 
                if ((altUI.Dados.ehCorreta && !altUI.IsSelected) || (!altUI.Dados.ehCorreta && altUI.IsSelected))
                {
                    acertouTudoNessaQuestao = false;
                }
            }

            if (!acertouTudoNessaQuestao) errouAlgumaNoQuizInteiro = true; 
        }
        else
        {
            // Avança para a próxima pergunta do array global
            indicePerguntaGlobal++;
            DefinirEConfigurarEtapaAtual();
        }
    }

    // =================================================================
    // PROCESSAMENTO: ARRASTAR E SOLTAR
    // =================================================================
    private void ConfigurarEtapaArrastar(QuizQuestion pergunta)
    {
        if (textoPerguntaDragUI != null) textoPerguntaDragUI.text = pergunta.textoPergunta;
        SpawnProximoObjetoDrag(pergunta);
    }

    private void SpawnProximoObjetoDrag(QuizQuestion pergunta)
    {
        if (indiceDragItemLocal < pergunta.itensParaArrastar.Length)
        {
            GameObject novoGo = Instantiate(prefabArrastavel, localSpawnElemento);
            novoGo.SetActive(true);
            
            QuizDragElement scriptElemento = novoGo.GetComponent<QuizDragElement>();
            scriptElemento.Configurar(pergunta.itensParaArrastar[indiceDragItemLocal], canvasPrincipal);
        }
        else
        {
            // Acabaram os itens dessa pergunta de arrastar! Avança no índice global
            indicePerguntaGlobal++;
            DefinirEConfigurarEtapaAtual();
        }
    }

    public void ProcessarDrop(QuizDragElement elemento, bool foiCorreto)
    {
        if (!foiCorreto) errouAlgumaNoQuizInteiro = true;

        elemento.ExecutarEfeitoEntrada();
        CanvasGroup painelAlvo = foiCorreto ? canvasGroupFeedbackCorreto : canvasGroupFeedbackErrado;

        if (painelAlvo != null)
            StartCoroutine(EfeitoPiscaLentoFeedback(painelAlvo));
        else
            StartCoroutine(AvançarDragSemFeedback());
    }

    private IEnumerator EfeitoPiscaLentoFeedback(CanvasGroup canvasGroupAlvo)
    {
        float tempo = 0;
        float duracaoFade = 0.4f;
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            canvasGroupAlvo.alpha = tempo / duracaoFade;
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        tempo = 0;
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            canvasGroupAlvo.alpha = 1f - (tempo / duracaoFade);
            yield return null;
        }
        canvasGroupAlvo.alpha = 0f;

        indiceDragItemLocal++;
        SpawnProximoObjetoDrag(quizAtual.perguntas[indicePerguntaGlobal]);
    }

    private IEnumerator AvançarDragSemFeedback()
    {
        yield return new WaitForSeconds(0.4f); 
        indiceDragItemLocal++;
        SpawnProximoObjetoDrag(quizAtual.perguntas[indicePerguntaGlobal]);
    }

    // =================================================================
    // FINALIZAÇÃO E REINÍCIO ENCAPSULADO
    // =================================================================
    private void FinalizarRodadaGeral()
    {
        abaMultiplaEscolha.SetActive(false);
        abaArrastar.SetActive(false);

        if (painelFimDeJogo != null) painelFimDeJogo.SetActive(true);
        if (botaoIrNovamente != null) botaoIrNovamente.SetActive(true);

        // REGRA DE OURO UNIFICADA: O botão de fechar (avançar na fase) só liga se não errou nada
        if (botaoFechar != null) 
        {
            botaoFechar.SetActive(!errouAlgumaNoQuizInteiro);
        }
    }

    public void ReiniciarQuizAtual()
    {
        IniciarQuizGeral(quizAtual);
    }
}