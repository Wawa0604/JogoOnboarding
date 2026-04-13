using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    private Camera _mainCamera;

    void Start()
    {
        // Cache da câmera principal para ganhar performance
        _mainCamera = Camera.main;
    }

    // Usamos LateUpdate para que o movimento ocorra APÓS a câmera se mover
    void LateUpdate()
    {
        if (_mainCamera == null) return;

        // 1. Pegamos a posição da câmera
        Vector3 targetPosition = _mainCamera.transform.position;

        // 2. Travamos o eixo Y para que seja igual ao do objeto (Sprite)
        // Isso impede que o objeto incline para frente ou para trás
        targetPosition.y = transform.position.y;

        // 3. Fazemos o objeto olhar para essa posição ajustada
        transform.LookAt(targetPosition);
    }
    
}
