using UnityEngine;
using System;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    [SerializeField] List<TabePageUIController> tabPageUIControllers = new List<TabePageUIController>();
    [SerializeField] List<TabSlot> tabSlots = new List<TabSlot>();
    
    // Lista de slots físicos na UI que representarão as cores (similar ao tabSlots)
    [SerializeField] List<CoresSprites> coresSlotsUI = new List<CoresSprites>();
    
    public event Action<SlotItemData> OnSlotButtonSelected;
    public event Action<TabPage> OnPageSelected;
    
    // Evento novo para notificar quando uma cor for escolhida
    public event Action<Color, string> OnColorSelected;

    internal TabePageUIController selectedTabePageUI; 

    private void Awake()
    {
        tabPageUIControllers.ForEach(tabPage =>
        {
            tabPage.SetVisibility(false); 
            tabPage.OnPageSelected += HandlePageSelected;
        });

        tabSlots.ForEach(slot =>
        {
            slot.SetVisibility(false); 
            slot.OnSlotButtonClicked += HandleSlotSelected;
        });

        // Inicializa os slots de cores ocultos e assina o evento de clique
        coresSlotsUI.ForEach(corSlot =>
        {
            corSlot.SetVisibility(false); 
            // Alterado: nome deve ser igual ao definido no script CoresSprites
            corSlot.colorSlotClicked += HandleCorSelected; 
        });
    }

    private void HandlePageSelected(TabePageUIController tabPageUIController)
    {
        selectedTabePageUI = tabPageUIController;

        tabPageUIControllers.ForEach(tabPageUI =>
        {
            if (tabPageUI == selectedTabePageUI)
            {
                tabPageUI.Selected(true); 
                SetUpTabSlots(tabPageUI.TabPage);
                
                // NOVO: Ao selecionar uma página, configuramos os slots de cores dela
                SetUpColors(tabPageUI.TabPage);

                int index = selectedTabePageUI.TabPage.selectedSlotIndex;
                if (index >= 0 && index < tabSlots.Count)
                {
                    SelectSlotButton(tabSlots[index]);
                }
                
                // NOVO: Seleciona visualmente a cor que estava salva na página (se houver)
                int colorIndex = selectedTabePageUI.TabPage.selectedColorIndex; 
                if (colorIndex >= 0 && colorIndex < coresSlotsUI.Count)
                {
                    SelectColorButton(coresSlotsUI[colorIndex]);
                }
            }
            else
            {
                tabPageUI.Selected(false);
            }
        });

        if (selectedTabePageUI != null)
        {
            OnPageSelected?.Invoke(selectedTabePageUI.TabPage);
        }
    }

    private void HandleSlotSelected(TabSlot tabSlot)
    {
        SelectSlotButton(tabSlot);

        SlotItemData slotItemData;
        slotItemData.sprite = tabSlot.Sprite;
        slotItemData.tabIdentifier = selectedTabePageUI.TabPage.identificador;
        OnSlotButtonSelected?.Invoke(slotItemData);
    }

    // NOVO: Lida com o clique no botão de cor
    private void HandleCorSelected(CoresSprites colorSlotClicked)
{
    // 1. Faz o loop para ligar/desligar o background dos botões
    SelectColorButton(colorSlotClicked);

    // 2. Avisa o sistema qual cor foi escolhida (usando a cor que guardamos no slot)
    // Supondo que você tenha esse evento Action<Color, string> OnColorSelected;
    OnColorSelected?.Invoke(colorSlotClicked.MinhaCor, selectedTabePageUI.TabPage.identificador);
}

    private void OnDestroy()
    {
        tabPageUIControllers.ForEach(tabPage => tabPage.OnPageSelected -= HandlePageSelected);
        tabSlots.ForEach(tabSlot => tabSlot.OnSlotButtonClicked -= HandleSlotSelected);
        // Limpeza dos eventos de cores
        coresSlotsUI.ForEach(corSlot => corSlot.colorSlotClicked -= HandleCorSelected);
    }

    private void SelectSlotButton(TabSlot tabSlot)
    {
       for (int i = 0; i < tabSlots.Count; i++)
        {
            var slot = tabSlots[i];
            if (slot == tabSlot)
            {
                slot.Select(true);
                if (selectedTabePageUI != null)
                {
                    selectedTabePageUI.TabPage.selectedSlotIndex = i;
                }
            }
            else
            {
                slot.Select(false);
            }
        } 
    }

    // NOVO: Lógica visual idêntica ao SelectSlotButton, mas para cores
    private void SelectColorButton(CoresSprites colorSlot)
    {
        for (int i = 0; i < coresSlotsUI.Count; i++)
        {
            // Se for o slot clicado, ele ativa o background. Senão, limpa.
            bool estaSelecionado = (coresSlotsUI[i] == colorSlot);
            coresSlotsUI[i].Select(estaSelecionado);

            // Salva o índice para quando você trocar de aba e voltar
            if (estaSelecionado && selectedTabePageUI != null)
            {
                selectedTabePageUI.TabPage.selectedColorIndex = i;
            }
        }
    }

    private void SetUpTabSlots(TabPage tabPage)
    {
        for (int i = 0; i < tabSlots.Count; i++)
        {
            var tabSlot = tabSlots[i];
            if(i < tabPage.sprites.Count)
            {
                tabSlot.SetVisibility(true);
                tabSlot.Sprite = tabPage.sprites[i];
            }
            else
            {
                tabSlot.SetVisibility(false);
            }
        }
    }

    
    private void SetUpColors(TabPage tabPage)
    {
        for (int i = 0; i < coresSlotsUI.Count; i++)
        {
            var slotUI = coresSlotsUI[i];

            // Se a aba usa cores e ainda temos cores na lista do ScriptableObject
            if (tabPage.useColor && i < tabPage.cores.Count)
            {
                slotUI.SetVisibility(true);
                slotUI.SetColor(tabPage.cores[i]); // Define a cor do ícone
                
                // Verifica se este slot era o que estava selecionado anteriormente
                slotUI.Select(i == tabPage.selectedColorIndex);
            }
            else
            {
                slotUI.SetVisibility(false);
            }
        }
    }

    public void AddTabPage(TabPage tabPage)
    {
        TabePageUIController tabePageUIController = tabPageUIControllers.Find(tab => !tab.IsVisible);
        if (tabePageUIController != null)
        {
            tabePageUIController.SetVisibility(true);
            tabePageUIController.TabPage = tabPage;
        }
    }

    public void SelectTabByIndex(int index)
    {
        if (index >= 0 && index < tabPageUIControllers.Count)
        {
            HandlePageSelected(tabPageUIControllers[index]);
        }
    }
}

public struct SlotItemData

{

    public string tabIdentifier;

    public Sprite sprite;

}