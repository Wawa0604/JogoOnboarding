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
            /// 1. Verificação de segurança
        if (todasAsMissoes.Count == 0) 
        {
            Debug.LogWarning("Aviso: Nenhuma missão foi atribuída à lista do MissionManager!");
            return 0;
        }

        // 2. Cálculos base
        int totalMissoes = todasAsMissoes.Count;
        int concluidas = todasAsMissoes.FindAll(m => m.completa).Count;
        
        // Quanto cada missão vale individualmente (ex: 100 / 4 = 25)
        float valorPorMissao = 100f / totalMissoes;
        
        // Porcentagem total atual
        float porcentagemTotal = ((float)concluidas / totalMissoes) * 100;

        // 3. DEBUG LOG DETALHADO
        Debug.Log("--- RELATÓRIO DE MISSÕES ---");
        foreach (var missao in todasAsMissoes)
        {
            string status = missao.completa ? "[CONCLUÍDA]" : "[PENDENTE]";
            Debug.Log($"Missão: {missao.id} | Status: {status} | Peso individual: {valorPorMissao}%");
        }
        Debug.Log($"TOTAL DE MISSÕES: {totalMissoes} | CONCLUÍDAS: {concluidas}");
        Debug.Log($"PORCENTAGEM ATUAL PARA O SCORM: {Mathf.RoundToInt(porcentagemTotal)}%");
        Debug.Log("---------------------------");

        // 4. Retorno final (encerra o método)
        return Mathf.RoundToInt(porcentagemTotal);
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