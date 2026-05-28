using UnityEngine;
using System;// precisa para usar o Action
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

    private void HandlePageSelected(TabePageUIController tabPageUIController) // precisa do parametro tabpageuicontroller pois o OnPageSelected ta dentro dele

    {
        selectedTabePageUI = tabPageUIController;// recebe o tab page ui controller
        // só pode ter uma aba selecionada por vez
        // então criamo o método que desativa todas as tabs diferentes dessa que estamos selecionando
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
            //por ultimo invocamos o OnPageSelected selecionando o tabpage
            OnPageSelected?.Invoke(selectedTabePageUI.TabPage);
        }
    }

    private void HandleSlotSelected(TabSlot tabSlot)
    {
        // Aqui chamamos passando o slot clicado

        SelectSlotButton(tabSlot);

        SlotItemData slotItemData;// por isso pegamos a referencia aqui do struct
        slotItemData.sprite = tabSlot.Sprite; // atualiza a variavel sprite que recebe o sprite dentro de slot selecionado
        slotItemData.tabIdentifier = selectedTabePageUI.TabPage.identificador;
        slotItemData.itemIndex = tabSlots.IndexOf(tabSlot);// encontra o índice do slot clicado na lista de slots ativos
        OnSlotButtonSelected?.Invoke(slotItemData);// espera o struct
    }


    // boa prática tirar as assinaturas de eventos que não estejam mais sendo utilizados
    private void OnDestroy()

    {
        tabPageUIControllers.ForEach(tabPage => tabPage.OnPageSelected -= HandlePageSelected);
        tabSlots.ForEach(tabSlot => tabSlot.OnSlotButtonClicked -= HandleSlotSelected);
    }

    private void SelectSlotButton(TabSlot tabSlot)

    {
        // for para navegar em cada slot
        // começando no i=0, indo até que o i seja menor que a quantidade de slots incrementando o i+1
       for (int i = 0; i < tabSlots.Count; i++)
        {
            // só um slot pode ser selecionado
            var slot = tabSlots[i];
            if (slot == tabSlot)
            {
                // verdadeiro se só um for selecionado
                slot.Select(true);
                // sempre q o slot for selecionado tem que atualizar o indice
                if (selectedTabePageUI != null)
                {
                    // atualiza o index no tabpage
                    selectedTabePageUI.TabPage.selectedSlotIndex = i;
                }
            }
            else
            {
                // se mais de um selecionado, é falso
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

    // método para adicionar uma tab e receber um objeto do tipo TabPage
    public void AddTabPage(TabPage tabPage)
    {
        //checar se existe uma tab disponivel
        TabePageUIController tabePageUIController = tabPageUIControllers.Find(tab => !tab.IsVisible);
        if (tabePageUIController != null) // caso retorne uma aba 
        {
            tabePageUIController.SetVisibility(true);// deixar ele visivel
            tabePageUIController.TabPage = tabPage;// atualiza o tab page
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

// struct para representar o objeto do slot, ao envez de classe
// da mesma forma que uma classe, ele também aceita atributos
// como se fosse uma classe mais simples que não precisa ser instanciada
// podemos alterar os dados diretamente na instancia da struct
// indicadas para representar dados simple como esses
public struct SlotItemData
{
    public string tabIdentifier;
    public Sprite sprite;
    public int itemIndex; //Adicionado para saber a posição do item
}

