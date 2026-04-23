using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Configurações de UI")]
    public TMP_InputField emailInput; // Pode deixar aqui para não dar erro de referência no Inspector
    public int nextSceneIndex;

    void Start()
    {
        // Ao iniciar a cena de Menu, ele chama automaticamente a entrada no jogo
        AutoEntrar();
    }

    private void AutoEntrar()
    {
        Debug.Log("SCORM detectado: Pulando tela de login...");
        
        // Se você ainda quiser salvar um identificador padrão no seu GameManager:
        if (GameManager.Instance != null)
        {
            // Você pode passar um ID genérico ou capturar do SCORM depois
            GameManager.Instance.SavePlayer("usuario_scorm"); 
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    // Mantemos este método apenas por segurança caso algum botão ainda o aponte, 
    // mas ele não será mais necessário na interface.
    public void EntrarNoJogo() 
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}