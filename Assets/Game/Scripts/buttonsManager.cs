using UnityEngine;

public class buttonsManager : MonoBehaviour
{
    // Arraste o objeto "PainelMissoes" para cá no Inspetor
    public GameObject painelMissoes; 
    public GameObject painelConfig;
    public GameObject painelCustomisacao;

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

    public void ToggleCustomisacao()
    {
        bool atual = painelCustomisacao.activeSelf;
        painelCustomisacao.SetActive(!atual);
    }
}
