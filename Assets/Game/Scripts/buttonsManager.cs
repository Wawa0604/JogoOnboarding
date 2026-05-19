using UnityEngine;

public class buttonsManager : MonoBehaviour
{
    // Arraste o objeto "PainelMissoes" para cá no Inspetor
    public GameObject painelMissoes; 
    public GameObject painelConfig;
    public GameObject painelCustomisacao;
    public GameObject painelVideo;

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

    public void DesligaCustomizacao()
{
    // 1. Fecha o painel de customização na ecrã (ação normal do botão)
    painelCustomisacao.SetActive(false);

    // 2. A VERIFICAÇÃO: Lemos o PlayerPrefs para saber o histórico do jogador
    // Pegamos no valor guardado na chave "ConversouNPC_MontarAvatar".
    // Se a chave não existir (porque o jogador ainda não falou com o NPC), ele assume o valor padrão: 0.
    int statusConversa = PlayerPrefs.GetInt("ConversouNPC_MontarAvatar", 0);

    // 3. A TOMADA DE DECISÃO (if):
    if (statusConversa == 1) 
    {
        // SE for igual a 1, significa que o NPC já ativou o gatilho!
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ConcluirMissao("montar_avatar");
            Debug.Log(" Sucesso! Missão 'montar_avatar' concluída porque falaste com o NPC antes.");
        }
    }
    else 
    {
        // SE for 0, significa que o jogador tentou fechar o painel sem falar com o NPC.
        Debug.Log(" O painel fechou, mas a missão NÃO foi concluída porque falta falar com o NPC.");
    }
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
