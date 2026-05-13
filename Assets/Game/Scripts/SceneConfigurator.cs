using UnityEngine;

public class SceneConfigurator : MonoBehaviour
{
    public static SceneConfigurator Instance;

    // Esta variável vai guardar a posição mesmo mudando de cena
    public Vector2 ultimaPosicaoSalva;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Este objeto fica na raiz e sobrevive
        }
        else
        {
            Destroy(gameObject);
        }
    }
}