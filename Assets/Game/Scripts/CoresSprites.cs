using UnityEngine;
using UnityEngine.UI;
using System;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground; // O "Frame" de seleção (Pai)
    [SerializeField] private Image buttonIcon;       // O círculo de cor (Filho)

    [SerializeField] private TabUIData tabData;
    [SerializeField] private int buttonIndex; // O índice deste botão (0, 1, 2...)

    // Singleton para facilitar o acesso do BodyUpdate ao evento de cor
    public static CoresSprites Instance;

    // O evento agora passa o Identificador (string) e a Cor (Color)
    public event Action<string, Color> OnColorChange;

    void Awake()
    {
        Instance = this;
        ApplyTabData();
    }

    private void ApplyTabData()
    {
        if (tabData == null) return;

        // Verifica se a aba deve usar cores e se este botão tem uma cor correspondente na lista
        if (tabData.useColor && tabData.colors != null)
        {
            if (buttonIndex < tabData.colors.Count)
            {
                // Aplica a cor da lista ao ícone (filho)
                buttonIcon.color = tabData.colors[buttonIndex];
            }
            else
            {
                // Caso existam mais botões que cores, você pode definir uma cor padrão ou desativar o ícone
                buttonIcon.color = Color.white; 
                gameObject.SetActive(false); // Opcional: desativa o botão se não houver cor para ele
            }
        }
    }

    public void OnClick()
    {
        // 1. Dispara o evento para quem estiver ouvindo (ex: BodyUpdate)
        // Precisamos do ID da aba atual. Se o TabUIData estiver preenchido:
        if (tabData != null)
        {
            OnColorChange?.Invoke(tabData.identificador, buttonIcon.color);
        }

        // 2. Lógica do Toggle de Alpha no Background
        // Como 'buttonBackground' já é do tipo Image, acessamos .color direto
        Color tempColor = buttonBackground.color;

        // Toggle do Alpha: se for 0, vira 1. Se for qualquer outra coisa, vira 0.
        tempColor.a = (tempColor.a == 0f) ? 1f : 0f;

        // Aplica de volta
        buttonBackground.color = tempColor;
    }
}