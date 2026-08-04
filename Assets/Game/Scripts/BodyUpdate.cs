using UnityEngine;
using System;
using UnityEngine.UI;

public class BodyUpdate : MonoBehaviour
{
    [SerializeField] private string identificador;
    [SerializeField] private bool dependeDoCorpo;   

    public static int CorpoAtualIndex = 0; 
    private int ultimoItemSelecionadoIndex = 0;

    private Color corSalva = Color.white;
    private bool temCorSalva = false;

    private void Start() 
    {
        TabsManager.Instance.OnBodyPartChange += HandleBodyPartChange;
        TabsManager.Instance.OnColorChange += HandleColorChange;

        int indexInicial = 0;

        // ==========================================
        // NOVO: Recupera o visual salvo para a nova cena!
        // ==========================================
        if (Game_Manager.Instance != null)
        {
            // Descobre o corpo global (Thin/Large)
            if (Game_Manager.Instance.avatarParts.ContainsKey("Body"))
            {
                CorpoAtualIndex = Game_Manager.Instance.avatarParts["Body"];
            }

            // Descobre o índice salvo desta peça específica
            if (Game_Manager.Instance.avatarParts.ContainsKey(identificador))
            {
                indexInicial = Game_Manager.Instance.avatarParts[identificador];
                ultimoItemSelecionadoIndex = indexInicial;
            }

            // Descobre a cor salva
            if (Game_Manager.Instance.avatarColors.ContainsKey(identificador))
            {
                corSalva = Game_Manager.Instance.avatarColors[identificador];
                corSalva.a = 1f;
                temCorSalva = true;
            }
        }

        // Força a inicialização usando o index recuperado do Game_Manager
        AtualizarFilhos(indexInicial); 
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
        }

        if (temCorSalva)
        {
            AplicarCorNaPecaAtiva();
        }
    }

    private void HandleColorChange(string idRecebido, Color corRecebida)
    {
        if (idRecebido == identificador)
        {
            corSalva = corRecebida;
            corSalva.a = 1f; 
            temCorSalva = true;
            AplicarCorNaPecaAtiva();
        }
    }

    private void AplicarCorNaPecaAtiva()
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
                    imgFilho.color = corSalva;
                }
                else
                {
                    SpriteRenderer srFilho = alvoDaCor.GetComponent<SpriteRenderer>();
                    if (srFilho != null)
                    {
                        srFilho.color = corSalva;
                    }
                }
                break; 
            }
        }
    }
}