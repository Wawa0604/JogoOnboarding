using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (_mainCamera == null) return;

        // 1. Calculamos a direção para a câmera
        Vector3 direction = _mainCamera.transform.position - transform.position;
        
        // 2. Calculamos qual seria a rotação necessária para olhar nessa direção
        Quaternion rotationToLook = Quaternion.LookRotation(direction);

        // 3. Pegamos a rotação ATUAL do objeto para preservar X e Z
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        // 4. Aplicamos a nova rotação:
        // Mantemos o X atual, pegamos o Y da câmera, mantemos o Z atual
        transform.rotation = Quaternion.Euler(currentEulerAngles.x, rotationToLook.eulerAngles.y, currentEulerAngles.z);
    }
}