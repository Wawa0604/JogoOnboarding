using UnityEngine;
using UnityEngine.UI;

public class Setas : MonoBehaviour
{
    public string idDaSeta; // Digite "1", "2" ou "Menu" aqui no Inspector
    public Button botaoAlvo;

    private void OnEnable()
    {
        // Quando a seta for ligada (SetActive(true)), ela começa a ouvir o botão
        if (botaoAlvo != null)
        {
            botaoAlvo.onClick.AddListener(DesativarSeta);
        }
    }

    private void OnDisable()
    {
        // Quando a seta for desligada, removemos o "ouvido" para evitar erros de memória
        if (botaoAlvo != null)
        {
            botaoAlvo.onClick.RemoveListener(DesativarSeta);
        }
    }

    private void DesativarSeta()
    {
        Debug.Log($"Botão {botaoAlvo.name} clicado. Desligando seta.");
        gameObject.SetActive(false);
    }
}
