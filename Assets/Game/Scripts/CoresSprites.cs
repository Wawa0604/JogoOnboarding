using UnityEngine;
using UnityEngine.UI;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground; 
    [SerializeField] private Image buttonIcon;       

    private Color currentColor; 
    
    // ==========================================
    // ESTA É A LINHA QUE FALTAVA: Permite que o TabsManager leia a cor deste botão
    // ==========================================
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
        TabsManager.Instance.NotifyColorClick(this, currentColor);
    }
}