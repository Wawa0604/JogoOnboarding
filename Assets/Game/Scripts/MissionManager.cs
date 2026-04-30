using UnityEngine;
using System.Collections;
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
        if (Instance == null) Instance = this;
        CarregarProgresso();
    }

    private void Start() => AtualizarInterface();

    public void ConcluirMissao(string id)
{
    MissaoData m = todasAsMissoes.Find(x => x.id == id);
    
    if (m != null && !m.completa)
    {
        m.completa = true;
        SalvarProgressoInterno(id);
        CalcularEEnviarProgressoSCORM();

        // Procura o item visual que representa esta missão para animar
        // Para isso funcionar, seus itens de UI precisam saber a qual ID pertencem
        // Uma forma simples é procurar pelo texto ou manter uma referência
        StartCoroutine(ExecutarAnimacaoVisual(id));
    }
}

private IEnumerator ExecutarAnimacaoVisual(string id)
{
    // Procura na hierarquia do container o item que tem o texto da missão concluída
    foreach (Transform child in containerLista)
    {
        ItemMissaoUI scriptItem = child.GetComponent<ItemMissaoUI>();
        MissaoData data = todasAsMissoes.Find(x => x.id == id);
        
        if (scriptItem != null && scriptItem.textoDescricao.text == data.descricao)
        {
            yield return StartCoroutine(scriptItem.AnimarConclusao());
            break;
        }
    }
}

    private void CalcularEEnviarProgressoSCORM()
    {
        int totalConcluido = 0;
        foreach (var m in todasAsMissoes)
        {
            if (m.completa) totalConcluido += m.pesoProgresso;
        }

        totalConcluido = Mathf.Clamp(totalConcluido, 0, 100);

        // CORREÇÃO DO WARNING AQUI:
        ScormManager scorm = Object.FindAnyObjectByType<ScormManager>();
        
        if (scorm != null)
        {
            scorm.SalvarProgressoFinal(totalConcluido);
        }
        else
        {
            Debug.LogWarning("MissionManager: ScormManager não encontrado na cena para salvar progresso.");
        }
    }

    public void AtualizarInterface()
    {
        // Limpa a lista
        foreach (Transform child in containerLista) Destroy(child.gameObject);

        // Cria uma cópia da lista para ordenar visualmente sem alterar a original
        List<MissaoData> ordenadas = new List<MissaoData>(todasAsMissoes);
        
        // Ordenação: Missões incompletas primeiro, completas vão para o fim
        ordenadas.Sort((a, b) => a.completa.CompareTo(b.completa));

        foreach (MissaoData m in ordenadas)
        {
            GameObject go = Instantiate(prefabMissao, containerLista);
            go.GetComponent<ItemMissaoUI>().Configurar(m.descricao, m.completa);
        }
    }

    void SalvarProgressoInterno(string id)
    {
        PlayerPrefs.SetInt("ProgressoMissao_" + id, 1);
        PlayerPrefs.Save();
    }

    void CarregarProgresso()
    {
        foreach (MissaoData m in todasAsMissoes)
        {
            m.completa = PlayerPrefs.GetInt("ProgressoMissao_" + m.id, 0) == 1;
        }
    }
}