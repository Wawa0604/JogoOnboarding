using UnityEngine;
using UnityEngine.UI;
public class LoadingScreenController : MonoBehaviour
{
    // Este método será chamado pelo evento da animação
    public void FinalizarCarregamento()
    {
        Image img = transform.parent.GetComponent<Image>();

        // Desliga o objeto pai (o painel de loading)
        transform.parent.gameObject.SetActive(false);
        // desativa o raycast target do pai
       if (img != null)
        {
            img.raycastTarget = false;
        }
    }
}

