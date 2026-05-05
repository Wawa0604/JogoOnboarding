using System;
using UnityEngine;
using UnityEngine.UI;

public class TabePageUIController : MonoBehaviour
{
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Image buttonIcon;  
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    
    private TabPage tabPage;
    public TabPage TabPage
    {
        get { return tabPage; }
        set 
        { 
            tabPage = value;
            buttonIcon.sprite = tabPage.icon;
        }
    }

    // Fixed: Ensure the Action matches the class name
    public event Action<TabePageUIController> OnPageSelected;

    public void Selected(bool selected)
    {
        buttonBackground.color = selected ? selectedColor : unselectedColor;
    }

    public bool IsVisible => gameObject.activeSelf;

    public void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void OnClick()
    {
        OnPageSelected?.Invoke(this);
    }
}