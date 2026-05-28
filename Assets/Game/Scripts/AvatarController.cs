using UnityEngine;
using System.Collections.Generic;

public class AvatarController : MonoBehaviour
{
    // Enum para deixar o código mais legível (0 = Magro, 1 = Cheinho)
    public enum TipoCorpo { Magro = 0, Cheinho = 1 }

    [System.Serializable]
    public class BodyPartGroup
    {
        public string tabIdentifier;   // ID idêntico ao ScriptableObject (ex: "corpo", "roupas")
        public Transform parentObject; // O objeto Pai na Hierarchy
        public bool dependeDoCorpo;    // Marque como TRUE se for roupas/acessórios que mudam com o corpo
    }

    [Header("Configuração das Partes do Avatar")]
    [SerializeField] private List<BodyPartGroup> bodyPartGroups = new List<BodyPartGroup>();

    private TipoCorpo corpoAtual = TipoCorpo.Magro;

    // Guarda o último índice que o jogador escolheu em cada aba (ex: "roupas" -> escolheu o índice 2)
    private Dictionary<string, int> itensEscolhidos = new Dictionary<string, int>();

    private void Start()
    {
        if (TabsManager.Instance != null)
        {
            TabsManager.Instance.OnBodyPartChange += HandleBodyPartChange;
        }
    }

    private void OnDestroy()
    {
        if (TabsManager.Instance != null)
        {
            TabsManager.Instance.OnBodyPartChange -= HandleBodyPartChange;
        }
    }

    private void HandleBodyPartChange(SlotItemData data)
    {
        // 1. Salva qual índice foi escolhido nesta categoria específica
        itensEscolhidos[data.tabIdentifier] = data.itemIndex;

        // 2. Se a aba alterada for a de CORPO, atualiza o tipo de corpo global
        // ATENÇÃO: Verifique se o identificador da sua aba de corpos é exatamente "corpo"
        if (data.tabIdentifier == "corpo")
        {
            corpoAtual = (TipoCorpo)data.itemIndex;

            // Como o corpo mudou, precisamos atualizar as roupas que já estavam vestidas para o novo tamanho
            AtualizarPartesDependentes();
        }

        // 3. Atualiza visualmente a categoria que foi clicada
        AtualizarVisualItem(data.tabIdentifier, data.itemIndex);
    }

    private void AtualizarVisualItem(string tabIdentifier, int itemIndex)
    {
        BodyPartGroup targetGroup = bodyPartGroups.Find(group => group.tabIdentifier == tabIdentifier);

        if (targetGroup == null || targetGroup.parentObject == null) return;

        int totalChildren = targetGroup.parentObject.childCount;
        for (int i = 0; i < totalChildren; i++)
        {
            GameObject child = targetGroup.parentObject.GetChild(i).gameObject;

            // Liga apenas a roupa/corpo correspondente ao Slot clicado
            bool deveAtivar = (i == itemIndex);
            child.SetActive(deveAtivar);

            // Se for a roupa escolhida E esse grupo depende do formato do corpo
            if (deveAtivar && groupDependeDoCorpo(targetGroup))
            {
                AjustarTamanhoDaRoupa(child.transform);
            }
        }
    }

    // Ativa o filho 0 (Magro) ou o filho 1 (Cheinho) dentro do objeto da roupa ligada
    private void AjustarTamanhoDaRoupa(Transform roupaObjeto)
    {
        int indexVariacao = (int)corpoAtual; // Converte o enum para 0 ou 1
        int totalVariacoes = roupaObjeto.childCount;

        for (int i = 0; i < totalVariacoes; i++)
        {
            // Ativa o modelo magro se indexVariacao for 0, ou cheinho se for 1
            roupaObjeto.GetChild(i).gameObject.SetActive(i == indexVariacao);
        }
    }

    // Percorre todas as categorias (como roupas) e força elas a se adaptarem ao novo corpo ativo
    private void AtualizarPartesDependentes()
    {
        foreach (var group in bodyPartGroups)
        {
            if (group.dependeDoCorpo && itensEscolhidos.ContainsKey(group.tabIdentifier))
            {
                int ultimoIndexDaRoupa = itensEscolhidos[group.tabIdentifier];
                AtualizarVisualItem(group.tabIdentifier, ultimoIndexDaRoupa);
            }
        }
    }

    private bool groupDependeDoCorpo(BodyPartGroup group)
    {
        return group.dependeDoCorpo;
    }
}