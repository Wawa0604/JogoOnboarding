using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCameraRotation : MonoBehaviour

{
    [Header("Configurações de Visão")]
    [SerializeField] private float mouseSensitivity = 0.1f;
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
    // Só aceita mouse
    if (!(_inputActions.Player.Look.activeControl?.device is Mouse)) return;
    if (!_isRightClicking) return;

    float lookX = _rotationInput.x * mouseSensitivity;
    float lookY = _rotationInput.y * mouseSensitivity;

    transform.Rotate(Vector3.up * lookX);

    _verticalRotation -= lookY;
    _verticalRotation = Mathf.Clamp(_verticalRotation, maxDownAngle, maxUpAngle);

    playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
}
}