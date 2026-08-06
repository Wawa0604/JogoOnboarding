using UnityEngine;
using UnityEngine.UI;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground; 
    [SerializeField] private Image buttonIcon;       

    private Color currentColor; 
    
    public Color CurrentColor => currentColor;

    public void Setup(Color cor)
    {
        currentColor = cor;
        cor.a = 1f; 
        buttonIcon.color = cor;
    }

    public void SetSelected(bool isSelected)
    {
        Color c = buttonBackground.color;
        c.a = isSelected ? 1f : 0f;
        buttonBackground.color = c;
    }

    public void OnClick()
    {
        // ATUALIZADO PARA A UNITY 6: FindAnyObjectByType
        TabsManager manager = FindAnyObjectByType<TabsManager>();
        if (manager != null)
        {
            manager.NotifyColorClick(this, currentColor);
        }
    }
}