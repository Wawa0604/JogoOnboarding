using UnityEngine;
using UnityEngine.Video; 
using UnityEngine.UI;
using System.IO; // IMPORTANTE: Necessário para juntar os caminhos da pasta

public class VideoController : MonoBehaviour
{
    [Header("Componente de Vídeo")]
    [SerializeField] private VideoPlayer videoPlayerComponente;
    
    // NOVO CAMPO: Digite apenas o nome do arquivo aqui no Inspector
    [Header("Arquivo de Vídeo")]
    [SerializeField] private string nomeDoVideo = "IMG_5017.mp4"; 

    [Header("UI da Barra de Progresso")]
    [SerializeField] private Image barraProgresso;

    [Header("UI do Botão Play (Feedback por Sprite)")]
    [SerializeField] private Image imagemBotaoPlay;
    [SerializeField] private Sprite spritePlay;    
    [SerializeField] private Sprite spritePause;    

    [Header("Botões de Fim de Vídeo")]
    [SerializeField] private GameObject botaoReassistir;
    [SerializeField] private GameObject botaoTerminar;
    [SerializeField] private GameObject botaoPlay;

    private void Awake()
    {
        // ASSINALA O CAMINHO CORRETO DINAMICAMENTE
        // Isso funciona perfeitamente no Editor e no WebGL!
        if (videoPlayerComponente != null)
        {
            videoPlayerComponente.url = Path.Combine(Application.streamingAssetsPath, nomeDoVideo);
        }
    }

    private void OnEnable()
    {
        OcultarBotoesDeFim();
        AlterarSpriteDoBotao(spritePlay); 

        if (videoPlayerComponente != null)
        {
            videoPlayerComponente.loopPointReached += AoTerminarOVideo;
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

    private void AtualizarBarraDeProgresso()
    {
        if (videoPlayerComponente != null && barraProgresso != null && videoPlayerComponente.length > 0)
        {
            float progresso = (float)(videoPlayerComponente.time / videoPlayerComponente.length);
            barraProgresso.fillAmount = progresso;
        }
    }

    public void AlternarPlayPause()
    {
        if (videoPlayerComponente == null) return;

        if (videoPlayerComponente.isPlaying)
        {
            videoPlayerComponente.Pause();
            AlterarSpriteDoBotao(spritePlay); 
            Debug.Log("<color=yellow>VideoController: Vídeo PAUSADO.</color>");
        }
        else
        {
            videoPlayerComponente.Play();
            AlterarSpriteDoBotao(spritePause); 
            OcultarBotoesDeFim(); 
            Debug.Log("<color=cyan>VideoController: Vídeo INICIADO / DESPAUSADO.</color>");
        }
    }

    private void AoTerminarOVideo(VideoPlayer vp)
    {
        if (botaoReassistir != null) botaoReassistir.SetActive(true);
        if (botaoTerminar != null) botaoTerminar.SetActive(true);
        if (botaoPlay != null) botaoPlay.SetActive(false);
        
        AlterarSpriteDoBotao(spritePlay); 
        
        if (barraProgresso != null) barraProgresso.fillAmount = 1f; 
        
        Debug.Log("<color=orange>VideoController: Vídeo chegou ao fim. Botões ativados!</color>");
    }

    public void ReassistirVideo()
    {
        if (videoPlayerComponente != null)
        {
            OcultarBotoesDeFim();
            videoPlayerComponente.Stop(); 
            videoPlayerComponente.Play();
            AlterarSpriteDoBotao(spritePause); 
            Debug.Log("<color=cyan>VideoController: Reiniciando o vídeo...</color>");
        }
    }

    private void OcultarBotoesDeFim()
    {
        if (botaoReassistir != null) botaoReassistir.SetActive(false);
        if (botaoTerminar != null) botaoTerminar.SetActive(false);
        if (botaoPlay != null) botaoPlay.SetActive(true);
    }

    private void AlterarSpriteDoBotao(Sprite novoSprite)
    {
        if (imagemBotaoPlay != null && novoSprite != null)
        {
            imagemBotaoPlay.sprite = novoSprite;
        }
    }
}