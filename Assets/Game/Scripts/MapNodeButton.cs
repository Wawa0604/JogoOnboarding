using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class MapNodeButton : MonoBehaviour
{
    [Header("Configuração do Destino")]
    public string nomeDaCena;

    private RectTransform rectTransform;
    private Button botao;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        botao = GetComponent<Button>();
        
        // Adiciona o listener automaticamente
        botao.onClick.AddListener(AoClicar);
    }

    public void AoClicar()
    {
        Debug.Log("O botão foi clicado! Tentando ir para: " + nomeDaCena); // <--- ADICIONE ESTA LINHA
        if (string.IsNullOrEmpty(nomeDaCena))
        {
            Debug.LogWarning("O nome da cena está vazio neste botão!");
            return;
        }

        // Dispara o evento avisando o avatar para onde ele deve ir
        GameEvents.OnTravelRequested?.Invoke(rectTransform.anchoredPosition, nomeDaCena);
    }
}