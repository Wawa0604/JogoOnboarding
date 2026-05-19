using UnityEngine;

public class VideoPlay : MonoBehaviour
{
    [Header("Painéis de Vídeo (Locais da Cena)")]
    [SerializeField] private GameObject painelVideoPresidente;

    private void OnEnable()
    {
        GameEvents.OnDialogueEnded += OuvirFimDoDialogue;
    }

    private void OnDisable()
    {
        GameEvents.OnDialogueEnded -= OuvirFimDoDialogue;
    }

    private void OuvirFimDoDialogue(string idDialogo)
    {
        if (idDialogo == "boasvindas_presidente")
        {
            if (painelVideoPresidente != null)
            {
                // Apenas liga o painel. O "OnEnable" do painel vai disparar o resto.
                painelVideoPresidente.SetActive(true);
                Debug.Log("<color=lime>VideoPlay: Painel de vídeo do Presidente ligado!</color>");
            }
            else
            {
                Debug.LogError("VideoPlay: O objeto do painel de vídeo não foi arrastado no Dialogue_Manager!");
            }
        }
    }
}