using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuizDragManager : MonoBehaviour
{
    [Header("Configuração de Paineis")]
    [SerializeField] private GameObject painelQuizArrastar;
    [SerializeField] private GameObject painelFimDeJogo;
    [SerializeField] private Canvas canvasPrincipal;

    [Header("Instanciamento")]
    [SerializeField] private GameObject prefabArrastavel;
    [SerializeField] private Transform localSpawnElemento; 

    [Header("Feedbacks Visuais (Com CanvasGroup)")]
    [SerializeField] private CanvasGroup canvasGroupFeedbackCorreto; // Imagem/Painel de Acerto
    [SerializeField] private CanvasGroup canvasGroupFeedbackErrado;  // Imagem/Painel de Erro

    [Header("Botões Finais")]
    [SerializeField] private GameObject botaoIrNovamente;
    [SerializeField] private GameObject botaoFechar;

    private QuizDragSequence quizAtual;
    private int indiceAtual;
    private bool errouAlguma;

    // CONECTA AO RÁDIO AO SER ATIVADO NA CENA
    private void OnEnable()
    {
        GameEvents.OnDragQuizRequested += IniciarQuiz;
        GameEvents.OnRestartDragQuizRequested += ReiniciarQuiz; // ➔ ESCUTA O REINÍCIO
    }

    private void OnDisable()
    {
        GameEvents.OnDragQuizRequested -= IniciarQuiz;
        GameEvents.OnRestartDragQuizRequested -= ReiniciarQuiz; // ➔ DESCONECTA POR SEGURANÇA
    }

    public void IniciarQuiz(QuizDragSequence novoQuiz)
    {
        // LOG DE ENTRADA:
        Debug.Log($"[RASTREIO 4] O QuizDragManager RECEBEU o sinal do rádio! Quiz: {(novoQuiz != null ? novoQuiz.id : "NULO")}");

        if (novoQuiz == null || novoQuiz.itensParaArrastar.Length == 0) return;
        quizAtual = novoQuiz;
        indiceAtual = 0;
        errouAlguma = false;

        painelQuizArrastar.SetActive(true);
        painelFimDeJogo.SetActive(false);
        
        // Garante que ambos os feedbacks comecem invisíveis
        if (canvasGroupFeedbackCorreto != null) canvasGroupFeedbackCorreto.alpha = 0f;
        if (canvasGroupFeedbackErrado != null) canvasGroupFeedbackErrado.alpha = 0f;

        SpawnProximoObjeto();
    }

    private void SpawnProximoObjeto()
    {
        // LOG DE CONTROLE: Mostra em qual item o jogador está
        Debug.Log($"[DRAG FIM] Itens processados: {indiceAtual} de {quizAtual.itensParaArrastar.Length}");

        if (indiceAtual < quizAtual.itensParaArrastar.Length)
        {
            GameObject novoGo = Instantiate(prefabArrastavel, localSpawnElemento);
            novoGo.SetActive(true);
            
            QuizDragElement scriptElemento = novoGo.GetComponent<QuizDragElement>();
            scriptElemento.Configurar(quizAtual.itensParaArrastar[indiceAtual], canvasPrincipal);
        }
        else
        {
            // LOG DE FLUXO: Chegou ao fim dos itens
            Debug.Log("[DRAG FIM] Todos os itens foram arrastados! Chamando FinalizarRodada().");
            FinalizarRodada();
        }
    }

    public void ProcessarDrop(QuizDragElement elemento, bool foiCorreto)
    {
        if (!foiCorreto) errouAlguma = true;

        // 1. Executa o efeito de sumir/encolher no elemento arrastado
        elemento.ExecutarEfeitoEntrada();

        // 2. Define dinamicamente qual painel vai piscar baseado no acerto/erro
        CanvasGroup painelAlvo = foiCorreto ? canvasGroupFeedbackCorreto : canvasGroupFeedbackErrado;

        if (painelAlvo != null)
        {
            // Dispara o pisca lento passando o painel certo por parâmetro
            StartCoroutine(EfeitoPiscaLentoFeedback(painelAlvo));
        }
        else
        {
            // Fallback de segurança caso você esqueça de arrastar um dos slots no Inspector
            StartCoroutine(AvançarSemFeedback());
        }
    }

    // Coroutine inteligente: funciona para qualquer um dos dois painéis
    private IEnumerator EfeitoPiscaLentoFeedback(CanvasGroup canvasGroupAlvo)
    {
        // Fade In (Ganhar opacidade suavemente)
        float tempo = 0;
        float duracaoFade = 0.4f; // Ajuste aqui para mudar a velocidade do pisca
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            canvasGroupAlvo.alpha = tempo / duracaoFade;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f); // Segura o feedback aceso na tela por um breve momento

        // Fade Out (Perder opacidade suavemente)
        tempo = 0;
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            canvasGroupAlvo.alpha = 1f - (tempo / duracaoFade);
            yield return null;
        }

        canvasGroupAlvo.alpha = 0f;

        // Avança para o próximo item da fila após o término do efeito visual
        indiceAtual++;
        SpawnProximoObjeto();
    }

    private IEnumerator AvançarSemFeedback()
    {
        yield return new WaitForSeconds(0.4f); 
        indiceAtual++;
        SpawnProximoObjeto();
    }

    private void FinalizarRodada()
    {
        Debug.Log("[DRAG FIM] Iniciou a execução do método FinalizarRodada().");

        if (painelQuizArrastar != null) 
        {
            painelQuizArrastar.SetActive(false);
        }

        // PROTEÇÃO E VALIDAÇÃO DO PAINEL FINAL
        if (painelFimDeJogo != null)
        {
            painelFimDeJogo.SetActive(true);
            Debug.Log("[DRAG FIM] O comando 'painelFimDeJogo.SetActive(true)' foi executado!");
        }
        else
        {
            Debug.LogError("[DRAG FIM] ERRO CRÍTICO: O slot 'Painel Fim De Jogo' está VAZIO no componente QuizDragManager no seu Inspector!");
        }

        // Proteções extras para os botões finais não quebrarem o código se estiverem vazios
        if (botaoIrNovamente != null) botaoIrNovamente.SetActive(true);
        if (botaoFechar != null) botaoFechar.SetActive(!errouAlguma);
    }

    public void ReiniciarQuiz() => IniciarQuiz(quizAtual);
}