using UnityEngine;

[CreateAssetMenu(fileName = "NovaMissao", menuName = "SistemaMissoes/Missao")]
public class MissaoData : ScriptableObject
{
    public string id; // ID único para salvar no PlayerPrefs
    public string descricao;
    public bool completa;
}
