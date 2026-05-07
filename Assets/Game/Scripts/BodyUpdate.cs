using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI; // precisa para usar o name space Image

public class BodyUpdate : MonoBehaviour
{
    [SerializeField] private string identificador;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        TabsManager.Instance.OnBodyPartChange += HandleBodPartChange;
    }

    private void HandleBodPartChange (SlotItemData slotItemData)
    {
        if(slotItemData.tabIdentifier == identificador)
        {
            img.sprite = slotItemData.sprite;
        }
    }
}
