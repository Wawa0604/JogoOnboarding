using UnityEngine;
using UnityEngine.UI;
using System;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground; // O "Frame" de seleção (Pai)
    [SerializeField] private Image buttonIcon;       // O círculo de cor (Filho)

    public event Action<CoresSprites> colorSlotClicked;

    // Propriedade para o Controller ler a cor deste botão
    public Color Color { get; private set; }

   public void SetColor(Color corVindaDoSO)
    {
        this.Color = corVindaDoSO; // Propriedade para o Controller ler depois
        buttonIcon.color = corVindaDoSO; // Onde a mágica acontece na UI
    }

    public void SetSelected(bool isSelected)
    {
        // Altera o alpha do background baseado na seleção
        Color c = buttonBackground.color;
        c.a = isSelected ? 1f : 0f;
        buttonBackground.color = c;
    }

    public void SetVisibility(bool value)
    {
        gameObject.SetActive(value);
    }

    public void OnClick()
    {
        colorSlotClicked?.Invoke(this);
    }
}