using UnityEngine;
using System;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    // controlador de todo o sistema de abas

//-------------- parte das abas/tabs -----------------------------------------------
    // criar um lugar para gardar todos os tab pages ui controllers
    [SerializeField] List<TabePageUIController> tabPageUIControllers = new List<TabePageUIController>();
   
    //variavel para guardar qual a aba selecionada no momento
    internal TabePageUIController selectedTabePageUI; // internal para fica só visivel para scripts
   
    // util para componentes externos que queiram saber quando uma aba for selecionada
    public event Action<TabPage> OnPageSelected;
    
// -------------parte dos slots -----------------------

    // lista para guardar a lista de slots
    [SerializeField] List<TabSlot> tabSlots = new List<TabSlot>();

    // evento que vai ser disparado sempre que um slot for selecionado
    public event Action<SlotItemData> OnSlotButtonSelected;
    
    private void Awake()
    {
        tabPageUIControllers.ForEach(tabPage =>
        {
            tabPage.SetVisibility(false);// oculta todas as abas
            tabPage.OnPageSelected += HandlePageSelected;// cria um método para sempre que uma aba for selecionada
        });

        // mesma função para o slot que nem a do page
        tabSlots.ForEach(slot =>
        {
            slot.SetVisibility(false);// oculta todas
            slot.OnSlotButtonClicked += HandleSlotSelected;// cria método para quando selecionado
        });
    }

    // Faltava este método que acabou se perdendo
    private void HandlePageSelected(TabePageUIController tabPageUIController)
    {
        selectedTabePageUI = tabPageUIController;
        
        tabPageUIControllers.ForEach(tabPageUI =>
        {
            if (tabPageUI == selectedTabePageUI)
            {
                tabPageUI.Selected(true);
                SetUpTabSlots(tabPageUI.TabPage);

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

    // Apenas UMA versão deste método
    private void HandleSlotSelected(TabSlot tabSlot)
    {
        SelectSlotButton(tabSlot);

        SlotItemData slotItemData;
        slotItemData.sprite = tabSlot.Sprite;
        
        // Pega as informações de origem do próprio slot
        slotItemData.tabIdentifier = tabSlot.groupIdentifier;
        slotItemData.itemIndex = tabSlot.indexInGroup; 
        
        OnSlotButtonSelected?.Invoke(slotItemData);
    }

    // boa prática tirar as assinaturas de eventos que não estejam mais sendo utilizados
    private void OnDestroy()
    {
        tabPageUIControllers.ForEach(tabPage => tabPage.OnPageSelected -= HandlePageSelected);
        tabSlots.ForEach(tabSlot => tabSlot.OnSlotButtonClicked -= HandleSlotSelected);
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

    private void SetUpTabSlots(TabPage tabPage)
    {
        int currentSlotIndex = 0;

        // Percorre todos os grupos dentro da aba
        foreach (var grupo in tabPage.grupos)
        {
            for (int i = 0; i < grupo.sprites.Count; i++)
            {
                if (currentSlotIndex < tabSlots.Count)
                {
                    var tabSlot = tabSlots[currentSlotIndex];
                    tabSlot.SetVisibility(true);
                    tabSlot.Sprite = grupo.sprites[i];
                    
                    // Salvamos a origem do dado direto no botão
                    tabSlot.groupIdentifier = grupo.identificador;
                    tabSlot.indexInGroup = i;
                    
                    currentSlotIndex++;
                }
            }
        }

        // Oculta os slots que não foram usados
        for (int i = currentSlotIndex; i < tabSlots.Count; i++)
        {
            tabSlots[i].SetVisibility(false);
        }
    }

    // método para adicionar uma tab e receber um objeto do tipo TabPage
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

// struct para representar o objeto do slot
public struct SlotItemData
{
    public string tabIdentifier;
    public Sprite sprite;
    public int itemIndex; //Adicionado para saber a posição do item
}