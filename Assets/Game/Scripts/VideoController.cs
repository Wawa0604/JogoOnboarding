using UnityEngine;
using UnityEngine.Video; 
using UnityEngine.UI; // IMPORTANTE: Adicionado para controlar Image e Sprites da UI

public class VideoController : MonoBehaviour
{
    [Header("Componente de Vídeo")]
    [SerializeField] private VideoPlayer videoPlayerComponente;

    [Header("UI da Barra de Progresso")]
    [SerializeField] private Image barraProgresso; // Arraste a imagem 'video_bar' para aqui

    [Header("UI do Botão Play (Feedback por Sprite)")]
    [SerializeField] private Image imagemBotaoPlay; // Arraste o componente Image do botão de Play aqui
    [SerializeField] private Sprite spritePlay;     // Arraste a imagem do ícone de Play (Triângulo)
    [SerializeField] private Sprite spritePause;    // Arraste a imagem do ícone de Pause (Duas barras)

    [Header("Botões de Fim de Vídeo")]
    [SerializeField] private GameObject botaoReassistir;
    [SerializeField] private GameObject botaoTerminar;

    private void OnEnable()
    {
        OcultarBotoesDeFim();
        AlterarSpriteDoBotao(spritePlay); // Garante que o botão comece com o ícone de Play

        if (videoPlayerComponente != null)
        {
            videoPlayerComponente.loopPointReached += AoTerminarOVideo;
            
            // Força o preview do primeiro frame
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

    private void Update()
    {
        AtualizarBarraDeProgresso();
    }

    /// <summary>
    /// Calcula a percentagem do vídeo e atualiza o preenchimento da barra horizontal.
    /// </summary>
    private void AtualizarBarraDeProgresso()
    {
        if (videoPlayerComponente != null && barraProgresso != null && videoPlayerComponente.length > 0)
        {
            // Calcula o progresso atual de 0.0 a 1.0
            float progresso = (float)(videoPlayerComponente.time / videoPlayerComponente.length);
            
            // Atualiza o Fill Amount da imagem da barra
            barraProgresso.fillAmount = progresso;
        }
    }

    public void AlternarPlayPause()
    {
        if (videoPlayerComponente == null) return;

        // 1. Se o vídeo JÁ ESTÁ rodando -> O jogador quer PAUSAR
        if (videoPlayerComponente.isPlaying)
        {
            videoPlayerComponente.Pause();
            AlterarSpriteDoBotao(spritePlay); // Muda o ícone para Play (avisando que o próximo clique vai dar play)
            Debug.Log("<color=yellow>VideoController: Vídeo PAUSADO.</color>");
        }
        // 2. Se o vídeo está parado ou pausado -> O jogador quer COMEÇAR / DESPAUSAR
        else
        {
            videoPlayerComponente.Play();
            AlterarSpriteDoBotao(spritePause); // Muda o ícone para Pause (avisando que o próximo clique vai pausar)
            OcultarBotoesDeFim(); 
            Debug.Log("<color=cyan>VideoController: Vídeo INICIADO / DESPAUSADO.</color>");
        }
    }

    private void AoTerminarOVideo(VideoPlayer vp)
    {
        if (botaoReassistir != null) botaoReassistir.SetActive(true);
        if (botaoTerminar != null) botaoTerminar.SetActive(true);
        
        AlterarSpriteDoBotao(spritePlay); // Reseta o ícone para Play quando acaba
        
        if (barraProgresso != null) barraProgresso.fillAmount = 1f; // Garante que a barra fica 100% cheia no fim
        
        Debug.Log("<color=orange>VideoController: Vídeo chegou ao fim. Botões ativados!</color>");
    }

    public void ReassistirVideo()
    {
        if (videoPlayerComponente != null)
        {
            OcultarBotoesDeFim();
            videoPlayerComponente.Stop(); 
            videoPlayerComponente.Play();
            AlterarSpriteDoBotao(spritePause); // Como começou a rodar, o ícone vira 'Pause'
            Debug.Log("<color=cyan>VideoController: Reiniciando o vídeo...</color>");
        }
    }

    private void OcultarBotoesDeFim()
    {
        if (botaoReassistir != null) botaoReassistir.SetActive(false);
        if (botaoTerminar != null) botaoTerminar.SetActive(false);
    }

    // MÈTODO AUXILIAR: Troca o sprite do botão de forma segura
    private void AlterarSpriteDoBotao(Sprite novoSprite)
    {
        if (imagemBotaoPlay != null && novoSprite != null)
        {
            imagemBotaoPlay.sprite = novoSprite;
        }
    }
}