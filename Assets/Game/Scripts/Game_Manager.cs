using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    [HideInInspector]
    public string currentPlayerEmail = "usuario_scorm";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayer(string email)
    {
        currentPlayerEmail = email;
        Debug.Log("Jogador definido como: " + email);
    }
}