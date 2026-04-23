using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para fácil acesso
    public string playerEmail;
    private ScormManager scorm;
    void Awake()
    {
        // Garante que só exista um GameManager e ele não seja destruído
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Carrega o e-mail salvo anteriormente, se existir
        playerEmail = PlayerPrefs.GetString("SavedEmail", "");
    }

    void Start() {
        scorm = GetComponent<ScormManager>(); // Pega o script que está no mesmo objeto
    }

    public void SavePlayer(string email)
    {
        playerEmail = email;
        PlayerPrefs.SetString("SavedEmail", email);
        PlayerPrefs.Save();
        Debug.Log("Progresso salvo para: " + email);
    }

}
