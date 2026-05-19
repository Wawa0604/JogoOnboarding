using UnityEngine;

public class buttonsManager : MonoBehaviour
{
    // Arraste o objeto "PainelMissoes" para cá no Inspetor
    public GameObject painelMissoes; 
    public GameObject painelConfig;
    public GameObject painelCustomisacao;
    public GameObject painelVideo;

    private const string CHAVE_CONVERSA_NPC = "ConversouNPC_MontarAvatar";

    // Quando o script acorda, ele inscreve-se no evento global de diálogos
    private void OnEnable()
    {
        GameEvents.OnDialogueEnded += VerificarFimDeDialogo;
    }

    // Se o script for destruído, ele limpa a inscrição para evitar erros de memória
    private void OnDisable()
    {
        GameEvents.OnDialogueEnded -= VerificarFimDeDialogo;
    }

    /// <summary>
    /// Esta função roda automaticamente SEMPRE que QUALQUER diálogo do jogo termina.
    /// </summary>
    private void VerificarFimDeDialogo(string idDialogo)
    {
        // Verificamos se o diálogo que terminou é o do nosso NPC (ID da foto)
        if (idDialogo == "boasvindas_colaboracao")
        {
            RegistrarConversaComNPC();
        }
    }

    public void RegistrarConversaComNPC()
    {
        PlayerPrefs.SetInt(CHAVE_CONVERSA_NPC, 1);
        PlayerPrefs.Save();
        Debug.Log(" Pré-requisito alcançado por Código: O jogador conversou com o NPC de colaboração!");
    }

    public void DesligaCustomizacao()
    {
        painelCustomisacao.SetActive(false);

        int statusConversa = PlayerPrefs.GetInt(CHAVE_CONVERSA_NPC, 0);

        if (statusConversa == 1) 
        {
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.ConcluirMissao("montar_avatar");
                Debug.Log(" Sucesso! Missão 'montar_avatar' concluída porque o evento do NPC foi detetado.");
            }
        }
        else 
        {
            Debug.Log(" O painel fechou, mas a missão NÃO foi concluída porque falta falar com o NPC.");
        }
    }

    void Start()
    {
        painelCustomisacao.SetActive(false);
    }

    public void TogglePainel()
    {
        // Inverte o estado atual (se está ativo, desativa; se está desativado, ativa)
        bool estadoAtual = painelMissoes.activeSelf;
        painelMissoes.SetActive(!estadoAtual);
    }

    public void ToggleConfig()
    {
        bool atual = painelConfig.activeSelf;
        painelConfig.SetActive(!atual);
    }

    public void DesligaConfig()
    {
        painelConfig.SetActive(false);
    }

    public void ToggleCustomisacao()
    {
        bool atual = painelCustomisacao.activeSelf;
        painelCustomisacao.SetActive(!atual);
    }

    public void LigaVideo()
    {
        painelVideo.SetActive(true);
    }

    public void DesligaVideo()
    {
        painelVideo.SetActive(false);
    }
}
