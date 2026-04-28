using UnityEngine;

[CreateAssetMenu(fileName = "NovaMissao", menuName = "SistemaMissoes/Missao")]
public class MissaoData : ScriptableObject
{
    public string id; 
    public string descricao;
    public int pesoProgresso = 10; // Quanto esta missão vale de 0 a 100
    
    [System.NonSerialized] // Garante que não salve no arquivo do SO durante o jogo
    public bool completa;
}