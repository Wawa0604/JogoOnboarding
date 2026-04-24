using UnityEngine;

public class MenuMissoes : MonoBehaviour
{
    // Arraste o objeto "PainelMissoes" para cá no Inspetor
    public GameObject painel; 

    public void TogglePainel()
    {
        // Inverte o estado atual (se está ativo, desativa; se está desativado, ativa)
        bool estadoAtual = painel.activeSelf;
        painel.SetActive(!estadoAtual);
    }
}