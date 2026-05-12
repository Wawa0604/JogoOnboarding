using UnityEngine;
using UnityEngine.UI;
using System;

public class CoresSprites : MonoBehaviour
{
    [SerializeField] private Image buttonBackground; // O "Frame" de seleção (Pai)
    [SerializeField] private Image buttonIcon;       // O círculo de cor (Filho)

    [SerializeField] private string currentId;
    [SerializeField] private Color currentColor; 

    
    // Chamado pelo TabsManager para "pintar" o botão
    public void Setup(string id, Color cor)
    {
        currentId = id;
        currentColor = cor;
        
        // 1. Mudamos a cor do ícone (o círculo colorido)
        // Garantimos que o alpha do ícone seja sempre 1 (100%) para ele aparecer
        cor.a = 1f; 
        buttonIcon.color = cor;
        
        // 2. Opcional: Se você quer que o FRAME (Background) comece invisível 
        // toda vez que troca de aba, mantenha as linhas abaixo.
        // Se quiser que ele mantenha o estado anterior, remova estas linhas:
        Color c = buttonBackground.color;
        c.a = 0f; // Começa transparente (toggle desligado)
        buttonBackground.color = c;
    }

    public void OnClick()
    {
        // Avisa o Manager que este botão foi clicado
        TabsManager.Instance.NotifyColorClick(currentId, currentColor);

        // Lógica do Toggle de Alpha no Background
        // Como 'buttonBackground' já é do tipo Image, acessamos .color direto
        Color tempColor = buttonBackground.color;

        // Toggle do Alpha: se for 0, vira 1. Se for qualquer outra coisa, vira 0.
        tempColor.a = (tempColor.a == 0f) ? 1f : 0f;

        // Aplica de volta
        buttonBackground.color = tempColor;
    }
}