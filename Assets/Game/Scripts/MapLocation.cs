using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MapLocation : MonoBehaviour
{
    [Header("Configuração do Destino")]
    public string nomeDaCena; // O nome exato da cena na Unity

    // Propriedade atalho para pegar o RectTransform do próprio botão
    public RectTransform RetornarRectTransform()
    {
        return GetComponent<RectTransform>();
    }
}