using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    [Header("Banco de Dados")]
    // Arraste todos os ScriptableObjects que você criar para esta lista no Inspector
    public List<CollectibleData> todosOsColetaveis;

    // Guarda apenas os IDs dos que já pegamos
    private HashSet<string> itensColetados = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Se estiver no Game_Manager que já dá DontDestroyOnLoad, não precisa fazer de novo aqui
            CarregarProgresso();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CarregarProgresso()
    {
        foreach (var item in todosOsColetaveis)
        {
            if (PlayerPrefs.GetInt("Coletavel_" + item.id, 0) == 1)
            {
                itensColetados.Add(item.id);
            }
        }
    }

    public void RegistrarColeta(string id)
    {
        // Se ainda não pegamos este item
        if (!itensColetados.Contains(id))
        {
            itensColetados.Add(id);
            PlayerPrefs.SetInt("Coletavel_" + id, 1);
            PlayerPrefs.Save();

            // Grita pro SCORM/UI que pegamos um item novo!
            GameEvents.OnItemCollected?.Invoke(id);
            
            Debug.Log($"<color=orange>[COLETÁVEL] Item {id} salvo na galeria!</color>");
        }
    }

    public bool JaFoiColetado(string id) => itensColetados.Contains(id);

    // Função pronta para o SCORM puxar
    public int ObterPorcentagemColetada()
    {
        if (todosOsColetaveis.Count == 0) return 0;
        float pct = ((float)itensColetados.Count / todosOsColetaveis.Count) * 100f;
        return Mathf.RoundToInt(pct);
    }
}