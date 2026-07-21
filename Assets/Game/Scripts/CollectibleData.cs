using UnityEngine;

[CreateAssetMenu(fileName = "NovoColetavel", menuName = "Sistema/Coletavel")]
public class CollectibleData : ScriptableObject
{
    [Header("Identificação")]
    public string id; // Ex: "colec_01"
    public string nomeItem;
    
    [Header("Conteúdo Visual")]
    [TextArea(3, 5)] 
    public string descricao;
    public Sprite foto;
    
    [Header("Links Externos")]
    public string urlExterno; // Ex: "https://www.cpqd.com.br"
}