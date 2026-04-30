using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

            // --- TRANSMISSÃO PARA O RÁDIO ---
            // O ScormManager vai ouvir isso aqui e atualizar a nota do aluno
            GameEvents.OnMissionCompleted?.Invoke(id);

            StartCoroutine(ExecutarAnimacaoVisual(id));
        }
    }

    // O ScormManager chama isso para saber o progresso atual
    public int ObterPorcentagemConcluida()
    {
        if (todasAsMissoes.Count == 0) return 0;

        float pesoTotal = 0;
        float pesoConcluido = 0;

        foreach (var m in todasAsMissoes)
        {
            pesoTotal += m.pesoProgresso;
            if (m.completa) pesoConcluido += m.pesoProgresso;
        }

        return pesoTotal > 0 ? Mathf.RoundToInt((pesoConcluido / pesoTotal) * 100) : 0;
    }

    private IEnumerator ExecutarAnimacaoVisual(string id)
    {
        MissaoData data = todasAsMissoes.Find(x => x.id == id);
        if (data == null) yield break;

        foreach (Transform child in containerLista)
        {
            ItemMissaoUI scriptItem = child.GetComponent<ItemMissaoUI>();
            
            // Encontra o item de UI que tem o texto da missão
            if (scriptItem != null && scriptItem.textoDescricao.text == data.descricao)
            {
                yield return StartCoroutine(scriptItem.AnimarConclusao());
                break;
            }
        }

        // Reordena a lista: completas vão para baixo automaticamente
        AtualizarInterface();
    }

    public void AtualizarInterface()
    {
        foreach (Transform child in containerLista) Destroy(child.gameObject);

        // Ordena: Incompletas em cima, Completas em baixo
        List<MissaoData> ordenadas = todasAsMissoes.OrderBy(m => m.completa).ToList();

        foreach (MissaoData m in ordenadas)
        {
            GameObject go = Instantiate(prefabMissao, containerLista);
            go.GetComponent<ItemMissaoUI>().Configurar(m.descricao, m.completa);
        }
    }

    private void SalvarProgressoInterno(string id)
    {
        PlayerPrefs.SetInt("ProgressoMissao_" + id, 1);
        PlayerPrefs.Save();
    }

    private void CarregarProgresso()
    {
        foreach (MissaoData m in todasAsMissoes)
            m.completa = PlayerPrefs.GetInt("ProgressoMissao_" + m.id, 0) == 1;
    }
}