using UnityEngine;

public class LoadingScreenController : MonoBehaviour
{
    // Este método será chamado pelo evento da animação
    public void FinalizarCarregamento()
    {
        // Desliga o objeto pai (o painel de loading)
        transform.parent.gameObject.SetActive(false);
    }
}

