using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; // precisa para usar o name space Image
using System; // necessário para usar o namespace Action

public class TabSlot : MonoBehaviour
{
    // referencia do background do botão
    [SerializeField] private Image buttonBackground;
    // referencia da imagem do botão
    [SerializeField] private Image buttonIcon;
    // cor de quando for selecionado
    [SerializeField] private Color selectedColor;
    // cor de quando não tiver selecionado
    [SerializeField] private Color unselectedColor;

    //propriedade do tipo sprite e sempre que ela for alterada, a imagem do botão vai ser alterada
    private Sprite sprite;
    public Sprite Sprite
    {
        get { return sprite; } 
        set { 
            sprite = value; 
            buttonIcon.sprite = sprite;
            }
    }

    // evento disparado sempre que o botão for clicado
    public event Action<TabSlot> OnSlotButtonClicked;
    
    // altera a cor do background do botão de acordo com o selected
    public void Select (bool selected)
    {
        buttonBackground.color = selected ? selectedColor : unselectedColor;
    }
    
    //método para alterar a visibilidade do botão
    public void SetVisibility(bool value)
    {
        gameObject.SetActive(value);
    }

    // metodo on click para rodar a visibilidade se for clicado
    public void OnClick()
    {
        OnSlotButtonClicked?.Invoke(this);
    }
}
