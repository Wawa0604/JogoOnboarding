using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// para que seja o primeiro rodado na cena
[DefaultExecutionOrder(-1)]

public class TabsManager : MonoBehaviour
{
     // Singleton pattern: onde só existe um tipo desse objeto na cena
    public static TabsManager Instance = null; 

    // referencia do tab ui controller para controlar o sistema de abas
    [SerializeField] private TabController tabController;
    // lista contendo as informações de cada tab
    [SerializeField] private List<TabUIData> data;
    // lista de botões de controllers
    [SerializeField] private List<Button> buttonColors = new List<Button>();

    public event Action<SlotItemData> OnBodyPartChange;


    private void Awake()
    {
            Instance = this;
    }

    void Start()
    {
        // inicializar as abas
        // para isso o for na lista de tabUIData
        data.ForEach(tabUIData =>
        {
            // chamando o método de tab page passando uma instancia de tab page
           tabController.AddTabPage(new TabPage()
           {
            // passando as informações 
                icon = tabUIData.icon,
                sprites = tabUIData.sprites,
                identificador = tabUIData.identificador,
           });
        });
        // manager precisa ser avisado quando uma tab é selecionada
        // assinar o on page selected e criar o método do handle
        // que será executado quando o evento for disparado
        tabController.OnPageSelected += HandlePageSelected;
        tabController.OnSlotButtonSelected += HandleSlotButtonSelected;
        tabController.SelectTabByIndex(0);
    }

    private void OnDestroy()
    {
        // removendo a subscrição do page selected
        tabController.OnPageSelected -= HandlePageSelected;
    }

    private void HandlePageSelected(TabPage obj)
    {
        
        Debug.Log("Aba selecionada: " + obj.identificador);
        var currentTabUIData = data.Find(tabUIData => tabUIData.identificador == obj.identificador);
        buttonColors.ForEach(buttonColor => buttonColor.gameObject.SetActive(currentTabUIData.useColor));
    }

    private void HandleSlotButtonSelected(SlotItemData obj)
    {
        TabsManager.Instance.OnBodyPartChange?.Invoke(obj);
    }

}