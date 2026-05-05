using UnityEngine;
using System;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    // Added 'e': TabePageUIController
    [SerializeField] List<TabePageUIController> tabPageUIControllers = new List<TabePageUIController>();
    
    // Added 'e': TabePageUIController
    internal TabePageUIController selectedTabePageUI; 
    
    public event Action<TabPage> OnPageSelected;

    private void Awake()
    {
        tabPageUIControllers.ForEach(tabPage =>
        {
            tabPage.SetVisibility(false); 
            tabPage.OnPageSelected += HandlePageSelected;
        });
    }

    private void OnDestroy()
    {
        tabPageUIControllers.ForEach(tabPage => tabPage.OnPageSelected -= HandlePageSelected);
    }

    // Added 'e': TabePageUIController
    private void HandlePageSelected(TabePageUIController tabPageUIController)
    {
        selectedTabePageUI = tabPageUIController;

        tabPageUIControllers.ForEach(tabPageUI =>
        {
            if (tabPageUI == selectedTabePageUI)
            {
                tabPageUI.Selected(true); 
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
}