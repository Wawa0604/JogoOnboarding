using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TabPage 
//contem as caracteristicasda aba
{
    // para cada tab teremos um identificador
    public string identificador;
    // um icone
    public Sprite icon;
    // uma lista de sprites que sera mostrada para cada tab
    public List<Sprite> sprites = new List<Sprite>();
    // variável que vai quardar qual o indice do slot selecionado
    public int selectedSlotIndex = 0;


}
