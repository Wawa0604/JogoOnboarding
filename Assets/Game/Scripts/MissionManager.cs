using UnityEngine;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Configurações de Dados")]
    public List<MissaoData> todasAsMissoes; 
    
    [Header("Configurações de UI")]
    public GameObject prefabMissao;      
    public Transform containerLista;     

    private void Awake()
    {
        // Configura o Singleton
        if (Instance == null) 
        {
            Instance = this;
        }
        
        CarregarProgresso();
    }

    private void Start() 
    {
        AtualizarInterface();
    }

    public void ConcluirMissao(string id)
    {
        // Procura a missão na lista pelo ID
        MissaoData m = todasAsMissoes.Find(x => x.id == id);
        
        if (m != null)
        {
            m.completa = true; // Usando 'completa' como no seu ScriptableObject
            SalvarProgresso(id);
            AtualizarInterface(); 
        }
    }

    public void AtualizarInterface()
    {
        // 1. Limpa a lista visual atual para evitar duplicatas
        foreach (Transform child in containerLista) 
        {
            Destroy(child.gameObject);
        }

        // 2. Recria os itens baseados nos dados atuais
        foreach (MissaoData m in todasAsMissoes)
        {
            GameObject go = Instantiate(prefabMissao, containerLista);
            
            // Passa os dados para o script do item
            go.GetComponent<ItemMissaoUI>().Configurar(m.descricao, m.completa);

            // Se a missão já foi terminada, joga para o fim da lista visual
            if (m.completa) 
            {
                go.transform.SetAsLastSibling();
            }
        }
    }

    // --- SALVAMENTO ---
    void SalvarProgresso(string id)
    {
        // Salva com um prefixo para não misturar com outros dados do jogo
        PlayerPrefs.SetInt("ProgressoMissao_" + id, 1);
        PlayerPrefs.Save();
    }

    void CarregarProgresso()
    {
        foreach (MissaoData m in todasAsMissoes)
        {
            // Se encontrar o valor 1 no PlayerPrefs, marca a missão como completa
            if (PlayerPrefs.GetInt("ProgressoMissao_" + m.id, 0) == 1)
            {
                m.completa = true;
            }
        }
    }
}