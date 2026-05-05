using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//caminho na unity para criar
[CreateAssetMenu(fileName = "TabUIData", menuName = "Scriptable Objects/TabUIData")]
public class TabUIData : ScriptableObject
{
    // icone que vai aparecer na aba
    public Sprite icon;
    public string identifier;
    //guardar a lista de sprites de cada aba
    public List<Sprite> sprites;
    //boleana para identificar se o objeto daquela aba vai ser alterado pela cor dela
    public bool useColor;

}
