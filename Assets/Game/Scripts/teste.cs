using UnityEngine;

public class Teste : MonoBehaviour
{
    public GameObject videoPainel;

    void Update()
    {
        // CORRIGIDO: Adicionado 'Input.', corrigida as maiúsculas e usado KeyCode.Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FechaVideoPainel();
        }
    }
    
    public void FechaVideoPainel()
    {
        // Proteção para só desligar se o painel realmente existir
        if (videoPainel != null)
        {
            videoPainel.SetActive(false);
        }
    }
}