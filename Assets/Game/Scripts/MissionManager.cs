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
    private Transform containerLista; // Removeu o [SerializeField] pois agora é automático     

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

    // ➔ O NOVO PORTAL DE CONEXÃO BLINDADO:
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
            GameEvents.OnMissionCompleted?.Invoke(id);
            
            if(containerLista != null)
                StartCoroutine(ExecutarAnimacaoVisual(id));
        }
    }

    public int ObterPorcentagemConcluida()
    {
        if (todasAsMissoes.Count == 0) return 0;

        int totalMissoes = todasAsMissoes.Count;
        int concluidas = todasAsMissoes.FindAll(m => m.completa).Count;
        float porcentagemTotal = ((float)concluidas / totalMissoes) * 100;

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