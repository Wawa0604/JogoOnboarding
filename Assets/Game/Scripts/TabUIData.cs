using UnityEngine;
using System.Collections.Generic;

// Classe que agrupa as informações de cada categoria dentro da aba
[System.Serializable]
public class AvatarGroupData
{
    public string identificador; // ex: "camisas", "jaquetas"
    public bool useColor;
    public List<Sprite> sprites;
}

[CreateAssetMenu(fileName = "TabUIData", menuName = "Scriptable Objects/TabUIData")]
public class TabUIData : ScriptableObject
{
    public Sprite icon;
    public string nomeDaAba; // Nome descritivo (opcional)
    
    [Header("Grupos desta Aba")]
    public List<AvatarGroupData> grupos; // Substitui o 'identificador' único
    
    [Header("Paleta de Cores da Aba")]
    public List<Color> colors; // Cores que serão compartilhadas
}