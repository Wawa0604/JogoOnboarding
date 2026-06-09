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
    [SerializeField] private Transform localSpawnElemento; // Onde o primeiro objeto vai nascer

    [Header("Feedback Visual")]
    [SerializeField] private CanvasGroup canvasGroupFeedbackCorreto; // Imagem de "Correto" com CanvasGroup

    [Header("Botões Finais")]
    [SerializeField] private GameObject botaoIrNovamente;
    [SerializeField] private GameObject botaoFechar;

    private QuizDragSequence quizAtual;
    private int indiceAtual;
    private bool errouAlguma;

    private void OnEnable()
    {
        // Registro no rádio (Mantenha o GameEvents atualizado com essa Action se for usar)
        // GameEvents.OnDragQuizRequested += IniciarQuiz;
    }

    public void IniciarQuiz(QuizDragSequence novoQuiz)
    {
        if (novoQuiz == null || novoQuiz.itensParaArrastar.Length == 0) return;

        quizAtual = novoQuiz;
        indiceAtual = 0;
        errouAlguma = false;

        painelQuizArrastar.SetActive(true);
        painelFimDeJogo.SetActive(false);
        if (canvasGroupFeedbackCorreto != null) canvasGroupFeedbackCorreto.alpha = 0f;

        SpawnProximoObjeto();
    }

    private void SpawnProximoObjeto()
    {
        if (indiceAtual < quizAtual.itensParaArrastar.Length)
        {
            GameObject novoGo = Instantiate(prefabArrastavel, localSpawnElemento);
            novoGo.SetActive(true);
            
            QuizDragElement scriptElemento = novoGo.GetComponent<QuizDragElement>();
            scriptElemento.Configurar(quizAtual.itensParaArrastar[indiceAtual], canvasPrincipal);
        }
        else
        {
            FinalizarRodada();
        }
    }

    public void ProcessarDrop(QuizDragElement elemento, bool foiCorreto)
    {
        if (!foiCorreto) errouAlguma = true;

        // 1. Executa o efeito de sumir/encolher no elemento arrastado
        elemento.ExecutarEfeitoEntrada();

        // 2. Se acertou, roda o efeito pisca lento do feedback de acerto
        if (foiCorreto && canvasGroupFeedbackCorreto != null)
        {
            StartCoroutine(EfeitoPiscaLentoFeedback());
        }
        else
        {
            // Se errou, não pisca o acerto, pula direto para o próximo após o tempo do encolhimento
            StartCoroutine(AvançarSemFeedback());
        }
    }

    private IEnumerator EfeitoPiscaLentoFeedback()
    {
        // Fade In (Ganhar opacidade)
        float tempo = 0;
        float duracaoFade = 0.5f;
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            canvasGroupFeedbackCorreto.alpha = tempo / duracaoFade;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f); // Segura aceso um pouquinho

        // Fade Out (Perder opacidade)
        tempo = 0;
        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            canvasGroupFeedbackCorreto.alpha = 1f - (tempo / duracaoFade);
            yield return null;
        }

        canvasGroupFeedbackCorreto.alpha = 0f;

        // Avança a fila
        indiceAtual++;
        SpawnProximoObjeto();
    }

    private IEnumerator AvançarSemFeedback()
    {
        yield return new WaitForSeconds(0.4f); // Espera o elemento terminar de encolher
        indiceAtual++;
        SpawnProximoObjeto();
    }

    private void FinalizarRodada()
    {
        painelQuizArrastar.SetActive(false);
        painelFimDeJogo.SetActive(true);

        botaoIrNovamente.SetActive(true);
        botaoFechar.SetActive(!errouAlguma); // Regra de ouro: Só fecha se o placar for perfeito
    }

    public void ReiniciarQuiz() => IniciarQuiz(quizAtual);
}