using UnityEngine;
using UnityEngine.Video; 
using TMPro; // IMPORTANTE: Adicionado para controlar o texto do botão

public class VideoController : MonoBehaviour
{
    [Header("Componente de Vídeo")]
    [SerializeField] private VideoPlayer videoPlayerComponente;

    [Header("UI do Botão Play (Opcional)")]
    [SerializeField] private TextMeshProUGUI textoBotaoPlay; // Arraste o componente de texto do botão de Play aqui

    [Header("Botões de Fim de Vídeo")]
    [SerializeField] private GameObject botaoReassistir;
    [SerializeField] private GameObject botaoTerminar;

    private void OnEnable()
    {
        OcultarBotoesDeFim();
        AlterarTextoDoBotao("Play"); // Garante que o botão comece escrito 'Play'

        if (videoPlayerComponente != null)
        {
            videoPlayerComponente.loopPointReached += AoTerminarOVideo;
            
            // --- NOVO: FORÇA O PREVIEW DO PRIMEIRO FRAME ---
            // Carrega o vídeo em segundo plano e renderiza o primeiro frame na tela,
            // mas mantém o vídeo pausado até o jogador clicar no botão de Play.
            videoPlayerComponente.Prepare();
        }
    }

    private void OnDisable()
    {
        if (videoPlayerComponente != null)
        {
            videoPlayerComponente.loopPointReached -= AoTerminarOVideo;
        }
    }

    // --- NOVA FUNÇÃO ALTERNÁVEL (PLAY / PAUSE / RESUME) ---
    // Substitua a antiga função 'PlayVideo' por esta no OnClick do seu botão
    public void AlternarPlayPause()
    {
        if (videoPlayerComponente == null) return;

        // 1. Se o vídeo JÁ ESTÁ rodando -> O jogador quer PAUSAR
        if (videoPlayerComponente.isPlaying)
        {
            videoPlayerComponente.Pause();
            AlterarTextoDoBotao("Play"); // Próximo clique vai despausar
            Debug.Log("<color=yellow>VideoController: Vídeo PAUSADO.</color>");
        }
        // 2. Se o vídeo está parado ou pausado -> O jogador quer COMEÇAR / DESPAUSAR
        else
        {
            videoPlayerComponente.Play();
            AlterarTextoDoBotao("Pause"); // Próximo clique vai pausar
            OcultarBotoesDeFim(); // Garante que esconde os botões finais se o vídeo recomeçar
            Debug.Log("<color=cyan>VideoController: Vídeo INICIADO / DESPAUSADO.</color>");
        }
    }

    private void AoTerminarOVideo(VideoPlayer vp)
    {
        if (botaoReassistir != null) botaoReassistir.SetActive(true);
        if (botaoTerminar != null) botaoTerminar.SetActive(true);
        AlterarTextoDoBotao("Play"); // Reseta o texto quando o vídeo acaba
        Debug.Log("<color=orange>VideoController: Vídeo chegou ao fim. Botões ativados!</color>");
    }

    public void ReassistirVideo()
    {
        if (videoPlayerComponente != null)
        {
            OcultarBotoesDeFim();
            videoPlayerComponente.Stop(); 
            videoPlayerComponente.Play();
            AlterarTextoDoBotao("Pause"); // Como começou a rodar, o botão vira 'Pause'
            Debug.Log("<color=cyan>VideoController: Reiniciando o vídeo...</color>");
        }
    }

    private void OcultarBotoesDeFim()
    {
        if (botaoReassistir != null) botaoReassistir.SetActive(false);
        if (botaoTerminar != null) botaoTerminar.SetActive(false);
    }

    // Método auxiliar para trocar o texto do botão de forma segura
    private void AlterarTextoDoBotao(string novoTexto)
    {
        if (textoBotaoPlay != null)
        {
            textoBotaoPlay.text = novoTexto;
        }
    }
}