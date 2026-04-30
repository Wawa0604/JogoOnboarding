using UnityEngine;

[CreateAssetMenu(fileName = "NovaMissao", menuName = "SistemaMissoes/Missao")]
public class MissaoData : ScriptableObject
{
    public string id; 
    public string descricao;
    [Range(0, 100)] public int pesoProgresso = 10; // O quanto essa missão ajuda a chegar no 100%
    
    [System.NonSerialized] 
    public bool completa;
}