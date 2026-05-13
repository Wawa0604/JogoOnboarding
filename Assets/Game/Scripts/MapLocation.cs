using UnityEngine;

public class MapLocation : MonoBehaviour
{
    [Header("Configuração do Destino")]
    public string nomeDaCena; // O nome exato da cena na Unity
    public float raioDeAtivacao = 50f; // O tamanho do seu "collider" invisível

    // Para nos ajudar a visualizar o collider no Editor da Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeAtivacao);
    }
}