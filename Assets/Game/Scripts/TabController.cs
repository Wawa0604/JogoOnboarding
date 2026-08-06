using UnityEngine;
using System;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    [SerializeField] List<TabePageUIController> tabPageUIControllers = new List<TabePageUIController>();
    internal TabePageUIController selectedTabePageUI; 
    public event Action<TabPage> OnPageSelected;
    
    [SerializeField] List<TabSlot> tabSlots = new List<TabSlot>();
    public event Action<SlotItemData> OnSlotButtonSelected;
    
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

    private void HandlePageSelected(TabePageUIController tabPageUIController)
    {
        selectedTabePageUI = tabPageUIController;
        
        tabPageUIControllers.ForEach(tabPageUI =>
        {
            if (tabPageUI == selectedTabePageUI)
            {
                tabPageUI.Selected(true);
                SetUpTabSlots(tabPageUI.TabPage);
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
        slotItemData.tabIdentifier = tabSlot.groupIdentifier;
        slotItemData.itemIndex = tabSlot.indexInGroup; 
        
        OnSlotButtonSelected?.Invoke(slotItemData);
    }

    private void OnDestroy()
    {
        tabPageUIControllers.ForEach(tabPage => tabPage.OnPageSelected -= HandlePageSelected);
        tabSlots.ForEach(tabSlot => tabSlot.OnSlotButtonClicked -= HandleSlotSelected);
    }

    private void SelectSlotButton(TabSlot tabSlot)
    {
        foreach (var slot in tabSlots)
        {
            if (!slot.gameObject.activeSelf) continue;

            if (slot.groupIdentifier == tabSlot.groupIdentifier)
            {
                slot.Select(slot == tabSlot);
            }
        }
    }

    private void SetUpTabSlots(TabPage tabPage)
    {
        int currentSlotIndex = 0;

        foreach (var grupo in tabPage.grupos)
        {
            for (int i = 0; i < grupo.sprites.Count; i++)
            {
                if (currentSlotIndex < tabSlots.Count)
                {
                    var tabSlot = tabSlots[currentSlotIndex];
                    tabSlot.SetVisibility(true);
                    tabSlot.Sprite = grupo.sprites[i];
                    tabSlot.groupIdentifier = grupo.identificador;
                    tabSlot.indexInGroup = i;
                    
                    currentSlotIndex++;
                }
            }
        }

        for (int i = currentSlotIndex; i < tabSlots.Count; i++)
        {
            tabSlots[i].SetVisibility(false);
        }

        SyncSlotVisuals();
    }

    public void SyncSlotVisuals()
    {
        // ATUALIZADO PARA A UNITY 6: FindAnyObjectByType
        TabsManager manager = FindAnyObjectByType<TabsManager>();
        if (manager == null) return;
        
        var savedParts = manager.GetCurrentParts();

        foreach (var slot in tabSlots)
        {
            if (!slot.gameObject.activeSelf) continue;

            int savedIndex = 0; 
            if (savedParts.ContainsKey(slot.groupIdentifier))
            {
                savedIndex = savedParts[slot.groupIdentifier];
            }

            slot.Select(slot.indexInGroup == savedIndex);
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
    public int itemIndex; 
}