using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Configurações de UI")]
    public TMP_InputField emailInput;
    public int nextSceneIndex;

    private int callCount = 0;

    public void EntrarNoJogo()
    {
        callCount++;

        Debug.Log("===== EntrarNoJogo CHAMADO =====");
        Debug.Log("Quantidade de chamadas: " + callCount);
        Debug.Log("Frame atual: " + Time.frameCount);
        Debug.Log("Email atual: " + emailInput.text);
        Debug.Log("Objeto que chamou: " + gameObject.name);
        Debug.Log("StackTrace:\n" + System.Environment.StackTrace);

        string email = emailInput.text.Trim().ToLower();

        if (email.EndsWith("@cpqd.com.br"))
        {
            Debug.Log("Email válido detectado");

            GameManager.Instance.SavePlayer(email);

            Debug.Log("Carregando cena...");
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Email inválido");
        }
    }
}