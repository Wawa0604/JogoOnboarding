using UnityEngine;
using System;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    [SerializeField] List<TabePageUIController> tabPageUIControllers = new List<TabePageUIController>();
    
    public event Action<SlotItemData> OnSlotButtonSelected;
    
    internal TabePageUIController selectedTabePageUI; 
    
    public event Action<TabPage> OnPageSelected;

    [SerializeField] List<TabSlot> tabSlots = new List<TabSlot>();

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
    }

    private void HandleSlotSelected(TabSlot tabSlot)
    {
        // Aqui chamamos passando o slot clicado
        SelectSlotButton(tabSlot);

        SlotItemData slotItemData;
        slotItemData.sprite = tabSlot.Sprite;
        slotItemData.tabIdentifier = selectedTabePageUI.TabPage.identificador;
        OnSlotButtonSelected?.Invoke(slotItemData);
    }

    private void OnDestroy()
    {
        tabPageUIControllers.ForEach(tabPage => tabPage.OnPageSelected -= HandlePageSelected);
        tabSlots.ForEach(tabSlot => tabSlot.OnSlotButtonClicked -= HandleSlotSelected);
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
                
                // CORREÇÃO: Passamos o slot da lista tabSlots, não do controller
                int index = selectedTabePageUI.TabPage.selectedSlotIndex;
                if (index >= 0 && index < tabSlots.Count)
                {
                    SelectSlotButton(tabSlots[index]);
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

    // CORREÇÃO AQUI: De 'TabSLot' para 'TabSlot' (letra l minúscula)
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