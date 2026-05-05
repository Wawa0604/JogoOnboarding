using UnityEngine;
using UnityEngine.UI;

public class Setas : MonoBehaviour
{
    public string idDaSeta;
    public Button botaoAlvo;

    [Header("Sequência")]
    [Tooltip("Arraste aqui a seta que deve ligar quando ESTA aqui sumir.")]
    [SerializeField] private GameObject proximaSeta; 

    private void OnEnable()
    {
        if (botaoAlvo != null) botaoAlvo.onClick.AddListener(DesativarSeta);
    }

    private void OnDisable()
    {
        if (botaoAlvo != null) botaoAlvo.onClick.RemoveListener(DesativarSeta);
    }

    private void DesativarSeta()
    {
        // 1. Liga a próxima seta (se houver uma configurada)
        if (proximaSeta != null)
        {
            proximaSeta.SetActive(true);
        }

        // 2. Desliga a si mesma
        gameObject.SetActive(false);
    }
}
