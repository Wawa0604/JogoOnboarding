using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;
using System.Text; 

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

    [Header("RELATÓRIO DE DESEMPENHO (Novos Slots)")]
    [SerializeField] private TextMeshProUGUI textoDesempenhoJogadorUI;
    [SerializeField] private TextMeshProUGUI textoDesempenhoIdealUI;

    // Variáveis de Controle de Fluxo
    private QuizSequence quizAtual;
    private int indicePerguntaGlobal; 
    private int indiceDragItemLocal;  
    private bool errouAlgumaNoQuizInteiro; 
    private bool exibindoJustificativa;
    
    // Contadores locais para o relatório
    private int dragItensCorretosNestaEtapa;
    private StringBuilder relatorioJogador = new StringBuilder();
    private StringBuilder relatorioIdeal = new StringBuilder();

    private List<QuizAlternativeUI> alternativasNaTela = new List<QuizAlternativeUI>();
    public bool ExibindoJustificativa => exibindoJustificativa;

    private void OnEnable()
    {
        GameEvents.OnQuizRequested += IniciarQuizGeral;
        GameEvents.OnRestartDragQuizRequested += ReiniciarQuizAtual; 
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

        relatorioJogador.Clear();
        relatorioIdeal.Clear();

        painelGeralQuiz.SetActive(true);
        if (painelFimDeJogo != null) painelFimDeJogo.SetActive(false);

        DefinirEConfigurarEtapaAtual();
    }

    private void DefinirEConfigurarEtapaAtual()
    {
        if (indicePerguntaGlobal >= quizAtual.perguntas.Length)
        {
            FinalizarRodadaGeral();
            return;
        }

        QuizQuestion etapaAtual = quizAtual.perguntas[indicePerguntaGlobal];

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
            indiceDragItemLocal = 0; 
            dragItensCorretosNestaEtapa = 0; 
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

            int totalCorretasDaEtapa = 0;
            int corretasMarcadasPeloJogador = 0;
            int erradasMarcadasPeloJogador = 0;

            // 1. Faz a varredura e contagem limpa dos botões
            foreach (QuizAlternativeUI altUI in alternativasNaTela)
            {
                altUI.RevelarResultado(); 
                
                if (altUI.Dados.ehCorreta) totalCorretasDaEtapa++;

                if (altUI.Dados.ehCorreta && altUI.IsSelected) corretasMarcadasPeloJogador++;
                if (!altUI.Dados.ehCorreta && altUI.IsSelected) erradasMarcadasPeloJogador++;
            }

            // 2. A MATEMÁTICA DA PRECISÃO: Separa substituições de excessos
            int corretasEsquecidas = totalCorretasDaEtapa - corretasMarcadasPeloJogador;
            
            // Quantas erradas apenas tentaram ocupar o lugar de uma certa que faltou
            int erradasQueSubstituem = Mathf.Min(erradasMarcadasPeloJogador, corretasEsquecidas);
            
            // Quantas erradas realmente passaram do limite total de respostas da pergunta
            int marcadasAMais = erradasMarcadasPeloJogador - erradasQueSubstituem;
            
            // Quantas certas ficaram em branco sem nenhuma errada tentando "substituí-las"
            int naoMarcadasPuras = corretasEsquecidas - erradasQueSubstituem;

            string resultadoTexto = "";
            int numeroEtapa = indicePerguntaGlobal + 1;

            // 3. MONTAGEM DO TEXTO BASEADO NOS SEUS EXEMPLOS
            
            // Cenário 100% Correto (Exemplos X e Z)
            if (corretasMarcadasPeloJogador == totalCorretasDaEtapa && erradasMarcadasPeloJogador == 0)
            {
                resultadoTexto = totalCorretasDaEtapa == 1 ? "Acertou" : $"{totalCorretasDaEtapa} Certas";
            }
            // Cenário 100% Errado puro (Exemplo B)
            else if (corretasMarcadasPeloJogador == 0 && erradasQueSubstituem == 1 && marcadasAMais == 0 && naoMarcadasPuras == 0)
            {
                resultadoTexto = "Errou";
            }
            // Cenários Mistos Customizados (Exemplos Y, A, C, D e o seu novo caso)
            else
            {
                List<string> partes = new List<string>();

                // Componente de acertos
                if (corretasMarcadasPeloJogador > 0) 
                {
                    partes.Add(corretasMarcadasPeloJogador == 1 ? "1 Correta" : $"{corretasMarcadasPeloJogador} Certas");
                }

                // Componente de erros que substituíram uma certa
                if (erradasQueSubstituem > 0) 
                {
                    partes.Add(erradasQueSubstituem == 1 ? "1 Errada" : $"{erradasQueSubstituem} Erradas");
                }

                // Componente de certas esquecidas puras (sem substituição)
                if (naoMarcadasPuras > 0) 
                {
                    partes.Add(naoMarcadasPuras == 1 ? "1 Não marcada" : $"{naoMarcadasPuras} Não marcadas");
                }

                // Componente de cliques que estouraram o orçamento
                if (marcadasAMais > 0) 
                {
                    partes.Add(marcadasAMais == 1 ? "1 Marcada a mais" : $"{marcadasAMais} Marcadas a mais");
                }

                resultadoTexto = string.Join(", ", partes);
            }

            // Gravação final nos painéis de texto do fim de jogo
            relatorioIdeal.AppendLine($"Pergunta {numeroEtapa}: Selecionar a(s) {totalCorretasDaEtapa} alternativa(s) correta(s)");
            relatorioJogador.AppendLine($"Pergunta {numeroEtapa}: {resultadoTexto}");

            // Validação para bloquear o avanço se houver qualquer deslize
            if (corretasMarcadasPeloJogador != totalCorretasDaEtapa || erradasMarcadasPeloJogador > 0)
            {
                errouAlgumaNoQuizInteiro = true;
            }
        }
        else
        {
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
            int numeroEtapa = indicePerguntaGlobal + 1;
            int totalItensEtapa = pergunta.itensParaArrastar.Length;
            int itensErrados = totalItensEtapa - dragItensCorretosNestaEtapa;

            string resultadoDragTexto = "";

            if (itensErrados == 0)
            {
                resultadoDragTexto = totalItensEtapa == 1 ? "Acertou" : $"{totalItensEtapa} Certas";
            }
            else if (dragItensCorretosNestaEtapa == 0)
            {
                resultadoDragTexto = itensErrados == 1 ? "Errou" : $"{itensErrados} Erradas";
            }
            else
            {
                resultadoDragTexto = $"{dragItensCorretosNestaEtapa} Certa(s), {itensErrados} Errada(s)";
            }

            relatorioIdeal.AppendLine($"Etapa {numeroEtapa} (Arrasto): Encaixar os {totalItensEtapa} itens locais corretos");
            relatorioJogador.AppendLine($"Etapa {numeroEtapa} (Arrasto): {resultadoDragTexto}");

            indicePerguntaGlobal++;
            DefinirEConfigurarEtapaAtual();
        }
    }

    public void ProcessarDrop(QuizDragElement elemento, bool foiCorreto)
    {
        if (foiCorreto)
        {
            dragItensCorretosNestaEtapa++;
        }
        else
        {
            errouAlgumaNoQuizInteiro = true;
        }

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

        if (botaoFechar != null) 
        {
            botaoFechar.SetActive(!errouAlgumaNoQuizInteiro);
        }

        if (textoDesempenhoJogadorUI != null) 
        {
            textoDesempenhoJogadorUI.text = relatorioJogador.ToString();
        }
        if (textoDesempenhoIdealUI != null) 
        {
            textoDesempenhoIdealUI.text = relatorioIdeal.ToString();
        }
    }

    public void ReiniciarQuizAtual()
    {
        IniciarQuizGeral(quizAtual);
    }

    public void EncerrarQuizComSucesso()
    {
        painelGeralQuiz.SetActive(false);
        GameEvents.OnQuizCompletedSuccessfully?.Invoke(quizAtual.id);
        Debug.Log("Quiz finalizado com sucesso! Voltando ao jogo.");
    }
}