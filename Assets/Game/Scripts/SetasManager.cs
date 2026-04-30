using UnityEngine;

public class SetasManager : MonoBehaviour
{
    public void AtivarSetaPorID(string idProcurado)
{
    Setas[] todasAsSetas = Resources.FindObjectsOfTypeAll<Setas>();

    foreach (Setas seta in todasAsSetas)
    {
        if (seta.idDaSeta == idProcurado) // Procura pela variável, não pela Tag
        {
            seta.gameObject.SetActive(true);
            return;
        }
    }
}
}