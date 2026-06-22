using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement; // IMPORTANTE: Adicionado para gerenciar cenas

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
    // Se o MissionManager tiver um pai (como o GameManager), usamos o objeto pai para o DontDestroyOnLoad
    GameObject objetoParaPersistir = transform.parent != null ? transform.parent.gameObject : gameObject;

    if (Instance == null) 
    {
        Instance = this;
        DontDestroyOnLoad(objetoParaPersistir); // Persiste o grupo inteiro corretamente
        CarregarProgresso();
    }
    else
    {
        Destroy(gameObject);
        return;
    }
}

    [Tooltip("Cole aqui o ID exato do QuizUnificado que finaliza esta missão.")]
    public string idQuizVinculado;

   // 1. Altera a função que recebe o evento do SceneManager
    private void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        // Em vez de chamar direto, iniciamos uma Coroutine para esperar o Unity estabilizar
        StartCoroutine(EsperarParaReconectar());
    }

    private IEnumerator EsperarParaReconectar()
    {
        // Espera até ao final do frame atual para que o Unity termine de carregar tudo
        yield return new WaitForEndOfFrame();
        ReconectarUI();
    }

    private void ReconectarUI()
    {
        // Esta versão busca inclusive em objetos desativados
        GameObject[] todosOsObjetos = Resources.FindObjectsOfTypeAll<GameObject>();
        GameObject painel = null;

        foreach (GameObject go in todosOsObjetos)
        {
            if (go.name == "fundo_painel" && go.scene == SceneManager.GetActiveScene())
            {
                painel = go;
                break;
            }
        }
        
        if (painel != null)
        {
            containerLista = painel.transform;
            Debug.Log("<color=cyan>MissionManager: Encontrado com sucesso!</color>");
            AtualizarInterface();
        }
        else
        {
            Debug.LogError("MissionManager: O objeto 'fundo_painel' não foi encontrado na cena atual!");
        }
    }
    // -------------------------------------------------------

    private void Start() 
    {
        if(containerLista != null) AtualizarInterface();
    }

    public void ConcluirMissao(string id)
    {
        MissaoData m = todasAsMissoes.Find(x => x.id == id);
        
        if (m != null && !m.completa)
        {
            m.completa = true;
            SalvarProgressoInterno(id);
            GameEvents.OnMissionCompleted?.Invoke(id);
            
            // Verificação de segurança para não quebrar a Coroutine se a UI sumir
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
        // Se não houver lista nesta cena (ex: menu inicial), não faz nada
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