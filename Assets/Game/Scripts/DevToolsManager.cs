using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

        // ➡️ Próxima cena (seta direita)
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            int nextScene = (currentSceneIndex + 1) % totalScenes;
            SceneManager.LoadScene(nextScene);
        }

        // ⬅️ Cena anterior (seta esquerda)
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            int prevScene = currentSceneIndex - 1;
            if (prevScene < 0) prevScene = totalScenes - 1;

            SceneManager.LoadScene(prevScene);
        }
    }
}