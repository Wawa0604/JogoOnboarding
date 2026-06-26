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
    private Transform containerLista; // Automático via MissionPanelUI

    private void Awake()
    {
        GameObject objetoParaPersistir = transform.parent != null ? transform.parent.gameObject : gameObject;

        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(objetoParaPersistir); 
            CarregarProgresso();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Chamado automaticamente pelo script MissionPanelUI de qualquer cena
    public void RegistrarContainer(Transform novoContainer)
    {
        containerLista = novoContainer;
        Debug.Log("<color=cyan>MissionManager: Nova UI de missões conectada com sucesso!</color>");
        
        // Força a interface a desenhar as missões daquela cena específica
        AtualizarInterface();
    }

    private void OnEnable()
    {
        GameEvents.OnQuizCompletedSuccessfully += AoConcluirQuizComSucesso; 
    }

    private void OnDisable()
    {
        GameEvents.OnQuizCompletedSuccessfully -= AoConcluirQuizComSucesso; 
    }

    private void AoConcluirQuizComSucesso(string idQuizVencido)
    {
        if (string.IsNullOrEmpty(idQuizVencido)) return;

        MissaoData missaoVinculada = todasAsMissoes.Find(x => x.idQuizVinculado == idQuizVencido);

        if (missaoVinculada != null)
        {
            Debug.Log($"<color=green>[MISSION] Quiz '{idQuizVencido}' concluído! Finalizando missão: '{missaoVinculada.id}'</color>");
            ConcluirMissao(missaoVinculada.id); 
        }
    }

    public void ConcluirMissao(string id)
    {
        MissaoData m = todasAsMissoes.Find(x => x.id == id);
        
        if (m != null && !m.completa)
        {
            m.completa = true;
            SalvarProgressoInterno(id);
            
            // Dispara o evento global de missão concluída
            GameEvents.OnMissionCompleted?.Invoke(id);
            
            // --- CONEXÃO COM O SCORM MANAGER PARA A NEOLUDE ---
            if (ScormManager.Instance != null)
            {
                ScormManager.Instance.DispararAtualizacaoLMS();
            }
            
            if (containerLista != null)
                StartCoroutine(ExecutarAnimacaoVisual(id));
        }
    }

    public int ObterPorcentagemConcluida()
    {
        if (todasAsMissoes == null || todasAsMissoes.Count == 0) return 0;

        int totalMissoes = todasAsMissoes.Count;
        
        // Conta sem alocar uma nova lista em memória
        int concluidas = todasAsMissoes.Count(m => m.completa); 
        
        float porcentagemTotal = ((float)concluidas / totalMissoes) * 100f;

        return Mathf.RoundToInt(porcentagemTotal);
    }

    private IEnumerator ExecutarAnimacaoVisual(string id)
    {
        MissaoData data = todasAsMissoes.Find(x => x.id == id);
        if (data == null || containerLista == null) yield break;

        foreach (Transform child in containerLista)
        {
            ItemMissaoUI scriptItem = child.GetComponent<ItemMissaoUI>();
            if (scriptItem != null && scriptItem.textoDescricao.text == data.descricao)
            {
                yield return StartCoroutine(scriptItem.AnimarConclusao());
                break;
            }
        }
        AtualizarInterface();
    }

    public void AtualizarInterface()
    {
        if (containerLista == null) return;

        foreach (Transform child in containerLista) Destroy(child.gameObject);

        // Mantém as ativas no topo e as completas embaixo
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