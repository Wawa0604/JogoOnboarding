using System; // necessário para usar o Action
using UnityEngine;
using UnityEngine.UI;// necessário para referenciar uma imagem

public class TabePageUIController : MonoBehaviour
{
    // imagem que vai representar a imagem de background do botão
    [SerializeField] private Image buttonBackground;
    // variavel para controlar o icone que vai aparecer na aba
    [SerializeField] private Image buttonIcon;  

    //duas cores de quando o botão estiver selecionado ou não
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    
    // propriedade do tipo tab page
    // sempre que ele or alterada, o icone será atualizado
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

    // evento que sempre será disparado quando o botão for clicado
    public event Action<TabePageUIController> OnPageSelected;

    // métodos que vão controlar o comportamento do tab page ui controller

    // método selected que que recebe o parametro tipo boleana
    //caso seja verdadeiro vai setar a cor do botão de acordo com selected color 
    // caso seja falso, setar cor como unselected color
    public void Selected(bool selected)
    {
        buttonBackground.color = selected ? selectedColor : unselectedColor;
    }

    // método que vai retornar se o objeto está ativo ou não
    public bool IsVisible => gameObject.activeSelf;

    // método que vai deixar visivel ou não o botão
    public void SetVisibility(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    // Passando o método que vai acionar o próprio objeto
    public void OnClick()
    {
        OnPageSelected?.Invoke(this);
    }

    // ? depois do objeto funciona como operador condicional nulo
    // na práticaé um atalho para checar se o onpageselected é nulo
    //caso eja não faz nada
    // caso tenha um valor, chama o método invoke

    // nesse caso especifico o c# só vai chamar se o evento for escutado por alguém
}