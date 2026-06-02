using UnityEngine;

public class AnimacaoSeta : MonoBehaviour
{
    [Header("Configurações de Flutuação")]
    [Tooltip("Velocidade do movimento de subida e descida.")]
    [SerializeField] private float speed = 3f;

    [Tooltip("A distância máxima que o objeto vai subir e descer a partir do ponto inicial.")]
    [SerializeField] private float amplitude = 0.2f;

    private float _initialY;

    void Start()
    {
        // Salva o Y inicial baseado na posição LOCAL (em relação ao pai ou ao mundo se não tiver pai)
        _initialY = transform.localPosition.y;
    }

    void Update()
    {
        // Mathf.Sin cria uma oscilação suave baseada no tempo
        float newY = _initialY + Mathf.Sin(Time.time * speed) * amplitude;

        // Aplica a nova posição mantendo o X e o Z intactos
        transform.localPosition = new Vector3(
            transform.localPosition.x, 
            newY, 
            transform.localPosition.z
        );
    }
}
