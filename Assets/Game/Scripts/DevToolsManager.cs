using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Importante: Adicione esta namespace

public class DevToolsManager : MonoBehaviour
{
    private static DevToolsManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Verifica se o teclado existe e se estamos em modo Debug/Editor
        if (Keyboard.current == null) return;

        if (Debug.isDebugBuild || Application.isEditor)
        {
            HandleSceneNavigation();
        }
    }

    private void HandleSceneNavigation()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // R - Próxima Cena (Equivalente ao GetKeyDown)
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            int nextScene = (currentSceneIndex + 1) % totalScenes;
            SceneManager.LoadScene(nextScene);
        }

        // E - Cena Anterior
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            int prevScene = currentSceneIndex - 1;
            if (prevScene < 0) prevScene = totalScenes - 1;
            
            SceneManager.LoadScene(prevScene);
        }
    }
}