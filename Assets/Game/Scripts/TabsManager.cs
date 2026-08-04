using UnityEngine;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class TabsManager : MonoBehaviour
{
    public static TabsManager Instance = null; 

    [SerializeField] private TabController tabController;
    [SerializeField] private List<TabUIData> data;
    [SerializeField] private List<CoresSprites> colorButtons = new List<CoresSprites>();

    public event Action<SlotItemData> OnBodyPartChange;
    public event Action<string, Color> OnColorChange;

    private TabUIData currentActiveTabData;
    private CoresSprites lastSelectedColorButton = null;

    private Dictionary<string, int> currentSelectedParts = new Dictionary<string, int>();
    private Dictionary<string, Color> currentSelectedColors = new Dictionary<string, Color>();

    private void Awake()
    {
        // Garante que a instância aponta para o TabsManager DA CENA ATUAL
        Instance = this;
    }

    void Start()
    {
        // ==========================================
        // NOVO: Recupera o progresso do Game_Manager para não salvar vazio!
        // ==========================================
        if (Game_Manager.Instance != null)
        {
            currentSelectedParts = new Dictionary<string, int>(Game_Manager.Instance.avatarParts);
            currentSelectedColors = new Dictionary<string, Color>(Game_Manager.Instance.avatarColors);
            Debug.Log("<color=green>[TabsManager]</color> Dicionários locais sincronizados com o Game_Manager.");
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

        for (int i = 0; i < colorButtons.Count; i++)
        {
            if (abaUsaCor && i < currentActiveTabData.colors.Count)
            {
                colorButtons[i].gameObject.SetActive(true);
                colorButtons[i].Setup(currentActiveTabData.colors[i]);
                colorButtons[i].SetSelected(false); 
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
                    OnColorChange?.Invoke(grupo.identificador, cor);
                    currentSelectedColors[grupo.identificador] = cor; 
                }
            }
        }
    }

    private void HandleSlotButtonSelected(SlotItemData obj)
    {
        OnBodyPartChange?.Invoke(obj);
        currentSelectedParts[obj.tabIdentifier] = obj.itemIndex;

        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ConcluirMissao("montar_avatar");
        }
    }

    public void SalvarCustomizacaoEFechar()
    {
        Debug.Log($"<color=green>[TabsManager]</color> Botão Salvar clicado! Enviando {currentSelectedParts.Count} peças e {currentSelectedColors.Count} cores para o Game_Manager.");
        GameEvents.OnAvatarSaved?.Invoke(currentSelectedParts, currentSelectedColors);
        
        gameObject.SetActive(false);
    }
}