using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    [Header("Receita do Item")]
    public CollectibleData data; // Arraste o ScriptableObject criado para cá!

    private void Start()
    {
        // Se já pegou antes, podemos destruir ou desativar direto no início
        if (CollectibleManager.Instance.JaFoiColetado(data.id))
        {
            gameObject.SetActive(false);
        }
    }

    // Chame este método a partir do seu sistema de cliques (Novo Input System ou OnMouseDown)
    public void AoSerClicado()
    {
        // 1. Mostra a UI bonita na tela
        if (CollectibleUIPopup.Instance != null)
        {
            CollectibleUIPopup.Instance.ExibirItem(data);
        }

        // 2. Salva no Manager e avisa os Eventos
        CollectibleManager.Instance.RegistrarColeta(data.id);

        // 3. Some da tela
        gameObject.SetActive(false);
    }
}