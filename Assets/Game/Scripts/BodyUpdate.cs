using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI; // precisa para usar o name space Image

public class BodyUpdate : MonoBehaviour
{
    [SerializeField] private string identificador; // Ex: "Cabelo", "Pele"
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        
    }

    private void Start() 
    {
        // Subscreve nos eventos
        TabsManager.Instance.OnBodyPartChange += HandleBodyPartChange;
        TabsManager.Instance.OnColorChange += HandleColorChange;
    }

    private void HandleColorChange(string idRecebido, Color corRecebida)
    {
        // Só muda a cor se o ID do botão clicado for igual ao ID desta parte do corpo
        if (idRecebido == identificador)
        {
            // Forçamos o Alpha para 1 para garantir que o sprite não fique invisível
            corRecebida.a = 1f; 
            img.color = corRecebida;
        }
    }

    private void HandleBodyPartChange (SlotItemData slotItemData)
    {
        if(slotItemData.tabIdentifier == identificador)
        {
            img.sprite = slotItemData.sprite;
        }
    }
}
