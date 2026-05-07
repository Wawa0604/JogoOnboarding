using UnityEngine;
using UnityEngine.UI;
using System;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground;
    [SerializeField] private Image buttonIcon; 

    // Propriedade para armazenar a cor que este slot representa (útil para o sistema saber qual cor foi enviada)
    public Color MinhaCor { get; private set; } 

    // Evento que avisa o TabController que ESTE botão foi clicado
    public event Action<CoresSprites> colorSlotClicked;

    // Método para o TabController definir a cor do ícone (vinda do TabUIData)
    public void SetColor(Color novaCor)
    {
        MinhaCor = novaCor;
        if (buttonIcon != null)
        {
            buttonIcon.color = novaCor; // O ícone assume a cor da lista
        }
        
        // Começa desmarcado (background transparente)
        Select(false);
    }

    // Controle do Background: Só aparece quando selecionado
    public void Select(bool isSelected)
    {
        if (buttonBackground != null)
        {
            // Se selecionado, Alpha em 1 (visível). Se não, Alpha em 0 (transparente).
            Color c = buttonBackground.color;
            c.a = isSelected ? 1f : 0f; 
            buttonBackground.color = c;
        }
    }

    public void SetVisibility(bool value)
    {
        gameObject.SetActive(value);
    }

    // Chamado pelo componente Button da Unity no OnClick()
    public void OnClick()
    {
        colorSlotClicked?.Invoke(this);
    }
}