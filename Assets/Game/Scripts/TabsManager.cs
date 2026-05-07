using UnityEngine;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class TabsManager : MonoBehaviour
{
    [SerializeField] private TabController tabController;
    [SerializeField] private List<TabUIData> data;
    public event Action<SlotItemData> OnBodyPartChange;

    // Singleton pattern
    public static TabsManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        data.ForEach(tabUIData =>
        {
           tabController.AddTabPage(new TabPage()
           {
                icon = tabUIData.icon,
                sprites = tabUIData.sprites,
                identificador = tabUIData.identificador,
           });
        });
        
        tabController.OnPageSelected += HandlePageSelected;
        tabController.OnSlotButtonSelected += HandleSlotButtonSelected;
        tabController.SelectTabByIndex(0);
    }

    private void OnDestroy()
    {
        if (tabController != null)
        {
            tabController.OnPageSelected -= HandlePageSelected;
        }
    }

    private void HandlePageSelected(TabPage obj)
    {
        
        Debug.Log("Aba selecionada: " + obj.identificador);
    }

    private void HandleSlotButtonSelected(SlotItemData obj)
    {
        TabsManager.Instance.OnBodyPartChange?.Invoke(obj);
    }
}