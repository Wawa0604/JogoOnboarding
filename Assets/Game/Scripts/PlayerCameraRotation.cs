using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCameraRotation : MonoBehaviour

{
    [Header("Configurações de Visão")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float arrowSensitivity = 2.0f;
    [SerializeField] private float maxUpAngle = 80f;
    [SerializeField] private float maxDownAngle = -80f;

    [Header("Referências")]
    [SerializeField] private Transform playerCamera;

    private PlayerControls _inputActions;
    private Vector2 _rotationInput;
    private float _verticalRotation = 0f;
    private bool _isRightClicking = false;

    void Awake()
    {
        // Inicializa a classe gerada pelo Input System
        _inputActions = new PlayerControls();

        // Configura os callbacks
        _inputActions.Player.Look.performed += ctx => _rotationInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Look.canceled += ctx => _rotationInput = Vector2.zero;

        // Se você criou a action "RightClick" no mapa "Player"
        _inputActions.Player.RightClick.performed += ctx => _isRightClicking = true;
        _inputActions.Player.RightClick.canceled += ctx => _isRightClicking = false;
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();

    void LateUpdate() // LateUpdate é melhor para câmeras para evitar trepidação
    {
        RotateLook();
    }

    private void RotateLook()
    {
        // Só rotaciona se houver input. 
        // E se o input vier do mouse, só rotaciona se o botão direito estiver pressionado.
        bool isMouse = _inputActions.Player.Look.activeControl?.device is Mouse;
        
        if (isMouse && !_isRightClicking) return;

        // Define qual sensibilidade usar
        float sensitivity = isMouse ? mouseSensitivity : arrowSensitivity;

        float lookX = _rotationInput.x * sensitivity;
        float lookY = _rotationInput.y * sensitivity;

        // 1. Girar o Player para os lados (Eixo Y)
        transform.Rotate(Vector3.up * lookX);

        // 2. Girar a Câmera para cima e para baixo (Eixo X) com trava (Clamp)
        _verticalRotation -= lookY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, maxDownAngle, maxUpAngle);
        
        playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }
}