using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TabPage 
{
    public string identificador;
    public Sprite icon;
    public List<Sprite> sprites = new List<Sprite>();
    public int selectedSlotIndex = 0;

    public int selectedColorIndex = 0;
    public List<Color> cores = new List<Color>();
    public bool useColor;
}
