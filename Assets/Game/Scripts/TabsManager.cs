using UnityEngine;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class TabsManager : MonoBehaviour
{
    // REMOVIDO: public static TabsManager Instance = null; 

    [SerializeField] private TabController tabController;
    [SerializeField] private List<TabUIData> data;
    [SerializeField] private List<CoresSprites> colorButtons = new List<CoresSprites>();

    private TabUIData currentActiveTabData;
    private CoresSprites lastSelectedColorButton = null;

    private Dictionary<string, int> currentSelectedParts = new Dictionary<string, int>();
    private Dictionary<string, Color> currentSelectedColors = new Dictionary<string, Color>();

    public Dictionary<string, int> GetCurrentParts() => currentSelectedParts;

    void Start()
    {
        if (Game_Manager.Instance != null)
        {
            currentSelectedParts = new Dictionary<string, int>(Game_Manager.Instance.avatarParts);
            currentSelectedColors = new Dictionary<string, Color>(Game_Manager.Instance.avatarColors);
        }

        data.ForEach(tabUIData =>
        {
            tabController.AddTabPage(new TabPage()
            {
                icon = tabUIData.icon,
                grupos = tabUIData.grupos 
            });
        });

        tabController.OnPageSelected += HandlePageSelected;
        tabController.OnSlotButtonSelected += HandleSlotButtonSelected;
        tabController.SelectTabByIndex(0);
    }

    private void OnDestroy()
    {
        if(tabController != null)
        {
            tabController.OnPageSelected -= HandlePageSelected;
            tabController.OnSlotButtonSelected -= HandleSlotButtonSelected;
        }
    }

    private void HandlePageSelected(TabPage obj)
    {
        currentActiveTabData = data.Find(t => t.icon == obj.icon); 
        if (currentActiveTabData == null) return;

        bool abaUsaCor = currentActiveTabData.grupos.Exists(g => g.useColor);
        lastSelectedColorButton = null; 

        Color corSalvaDaAba = Color.clear;
        bool temCorSalva = false;
        foreach (var grupo in currentActiveTabData.grupos)
        {
            if (grupo.useColor && currentSelectedColors.ContainsKey(grupo.identificador))
            {
                corSalvaDaAba = currentSelectedColors[grupo.identificador];
                temCorSalva = true;
                break;
            }
        }

        for (int i = 0; i < colorButtons.Count; i++)
        {
            if (abaUsaCor && i < currentActiveTabData.colors.Count)
            {
                colorButtons[i].gameObject.SetActive(true);
                colorButtons[i].Setup(currentActiveTabData.colors[i]);

                if (temCorSalva && colorButtons[i].CurrentColor == corSalvaDaAba)
                {
                    colorButtons[i].SetSelected(true);
                    lastSelectedColorButton = colorButtons[i];
                }
                else
                {
                    colorButtons[i].SetSelected(false);
                }
            }
            else
            {
                colorButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void NotifyColorClick(CoresSprites clickedButton, Color cor)
    {
        if (lastSelectedColorButton != null)
        {
            lastSelectedColorButton.SetSelected(false);
        }
        clickedButton.SetSelected(true);
        lastSelectedColorButton = clickedButton;

        if (currentActiveTabData != null)
        {
            foreach (var grupo in currentActiveTabData.grupos)
            {
                if (grupo.useColor)
                {
                    GameEvents.OnPreviewColorChanged?.Invoke(grupo.identificador, cor);
                    currentSelectedColors[grupo.identificador] = cor; 
                }
            }
        }
    }

    private void HandleSlotButtonSelected(SlotItemData obj)
    {
        GameEvents.OnPreviewBodyPartChanged?.Invoke(obj);
        currentSelectedParts[obj.tabIdentifier] = obj.itemIndex;

        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ConcluirMissao("montar_avatar");
        }
    }

    public void SalvarCustomizacaoEFechar()
    {
        Debug.Log($"<color=yellow>[TabsManager]</color> Botão clicado! Preparando para enviar {currentSelectedParts.Count} peças e {currentSelectedColors.Count} cores.");
        
        if (GameEvents.OnAvatarSaved != null)
        {
            GameEvents.OnAvatarSaved.Invoke(currentSelectedParts, currentSelectedColors);
            Debug.Log("<color=green>[TabsManager]</color> Evento de salvamento enviado com sucesso!");
        }
        else
        {
            Debug.LogError("<color=red>[TabsManager]</color> ERRO: O evento foi chamado, mas ninguém (Game_Manager) está escutando!");
        }
    }
}