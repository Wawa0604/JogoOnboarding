using UnityEngine;
using System;
using UnityEngine.UI;

public class BodyUpdate : MonoBehaviour
{
    [SerializeField] private string identificador; // Ex: "corpo", "roupas"
    [SerializeField] private bool dependeDoCorpo;    // Marque True no Inspector apenas na aba de roupas

    // Variável estática/global para que todas as roupas saibam qual corpo está ativo no momento
    public static int CorpoAtualIndex = 0; 
    private int ultimoItemSelecionadoIndex = 0;

    private void Start() 
    {
        TabsManager.Instance.OnBodyPartChange += HandleBodyPartChange;
        TabsManager.Instance.OnColorChange += HandleColorChange;

        // Força a inicialização correta no primeiro frame
        AtualizarFilhos(0); 
    }

    private void OnDestroy()
    {
        if (TabsManager.Instance != null)
        {
            TabsManager.Instance.OnBodyPartChange -= HandleBodyPartChange;
            TabsManager.Instance.OnColorChange -= HandleColorChange;
        }
    }

    private void HandleBodyPartChange(SlotItemData slotItemData)
    {
        if (slotItemData.tabIdentifier == "Body") 
        {
            CorpoAtualIndex = slotItemData.itemIndex;
            
            if (dependeDoCorpo && ultimoItemSelecionadoIndex != -1)
            {
                AtualizarFilhos(ultimoItemSelecionadoIndex);
            }
        }

        if (slotItemData.tabIdentifier == identificador)
        {
            ultimoItemSelecionadoIndex = slotItemData.itemIndex;
            AtualizarFilhos(slotItemData.itemIndex);
        }
    }

    private void AtualizarFilhos(int itemIndex)
    {
        int totalFilhos = transform.childCount;
        
        for (int i = 0; i < totalFilhos; i++)
        {
            GameObject filho = transform.GetChild(i).gameObject;
            bool deveAtivar = (i == itemIndex);

            if (deveAtivar && dependeDoCorpo)
            {
                int totalSubFilhos = filho.transform.childCount;
                for (int j = 0; j < totalSubFilhos; j++)
                {
                    GameObject subFilho = filho.transform.GetChild(j).gameObject;
                    bool deveAtivarSub = (j == CorpoAtualIndex);
                    subFilho.SetActive(deveAtivarSub); 
                }
            }

            filho.SetActive(deveAtivar);

            if (deveAtivar)
            {
                Debug.Log($"<color=orange>[Categoria: {identificador}]</color> Peça principal ativada: <b>{filho.name}</b>");
                if (dependeDoCorpo)
                {
                    for (int j = 0; j < filho.transform.childCount; j++)
                    {
                        GameObject subFilho = filho.transform.GetChild(j).gameObject;
                        string nomeSprite = "Sem imagem";
                        Image img = subFilho.GetComponent<Image>();
                        if (img != null && img.sprite != null) nomeSprite = img.sprite.name;
                        
                        Debug.Log($"   -> Sub-objeto: <b>{subFilho.name}</b> | Ativo no Jogo: <b>{subFilho.activeSelf}</b> | Imagem usada: <color=yellow><b>{nomeSprite}</b></color>");
                    }
                }
            }
        }
    }

    // =======================================================================
    // DETETIVE DE FRAME (Pega no flagra quem está alterando os objetos por fora)
    // =======================================================================
    private float proximoAvisoTempo = 0f;
    private void Update()
    {
        if (dependeDoCorpo && ultimoItemSelecionadoIndex >= 0 && ultimoItemSelecionadoIndex < transform.childCount)
        {
            GameObject roupaAtiva = transform.GetChild(ultimoItemSelecionadoIndex).gameObject;
            
            if (roupaAtiva.activeSelf && roupaAtiva.transform.childCount > 0)
            {
                for (int j = 0; j < roupaAtiva.transform.childCount; j++)
                {
                    GameObject subFilho = roupaAtiva.transform.GetChild(j).gameObject;
                    bool deveriaEstarAtivo = (j == CorpoAtualIndex);
                    
                    // Se o estado real do objeto na Unity NÃO bate com o que o script definiu:
                    if (subFilho.activeSelf != deveriaEstarAtivo && Time.time > proximoAvisoTempo)
                    {
                        Debug.LogError($"<color=red><b>[INVERSÃO DETECTADA!]</b></color> No objeto <b>{roupaAtiva.name}</b>, o sub-filho <b>{subFilho.name}</b> foi forçado para Ativo={subFilho.activeSelf}! " +
                                       $"Isso confirma que existe algo externo modificando este objeto.");
                        proximoAvisoTempo = Time.time + 1f; // Evita travar o console com mensagens infinitas
                    }
                }
            }
        }
    }

    private void HandleColorChange(string idRecebido, Color corRecebida)
    {
        if (idRecebido == identificador)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform filho = transform.GetChild(i);
                if (filho.gameObject.activeSelf)
                {
                    Transform alvoDaCor = filho;
                    if (dependeDoCorpo)
                    {
                        for (int j = 0; j < filho.childCount; j++)
                        {
                            if (filho.GetChild(j).gameObject.activeSelf)
                            {
                                alvoDaCor = filho.GetChild(j);
                                break;
                            }
                        }
                    }

                    Image imgFilho = alvoDaCor.GetComponent<Image>();
                    if (imgFilho != null)
                    {
                        corRecebida.a = 1f;
                        imgFilho.color = corRecebida;
                        break;
                    }

                    SpriteRenderer srFilho = alvoDaCor.GetComponent<SpriteRenderer>();
                    if (srFilho != null)
                    {
                        corRecebida.a = 1f;
                        srFilho.color = corRecebida;
                        break;
                    }
                }
            }
        }
    }
}