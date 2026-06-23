using UnityEngine;

public class MissionPanelUI : MonoBehaviour
{
    private void Start()
    {
        // Assim que o objeto acorda na nova cena, ele se entrega para o Manager
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.RegistrarContainer(transform);
        }
    }
}