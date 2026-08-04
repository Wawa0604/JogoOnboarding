using UnityEngine;
using UnityEngine.UI;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground; 
    [SerializeField] private Image buttonIcon;       

    private Color currentColor; 

    public void Setup(Color cor)
    {
        currentColor = cor;
        cor.a = 1f; 
        buttonIcon.color = cor;
    }

    // Chamado pelo TabsManager para ligar/desligar a bordinha
    public void SetSelected(bool isSelected)
    {
        Color c = buttonBackground.color;
        c.a = isSelected ? 1f : 0f;
        buttonBackground.color = c;
    }

    public void OnClick()
    {
        // Envia este botão como referência para o Manager deselecionar os outros
        TabsManager.Instance.NotifyColorClick(this, currentColor);
    }
}