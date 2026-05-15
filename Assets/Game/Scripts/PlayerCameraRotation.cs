using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraRotation : MonoBehaviour
{
    [Header("Configurações de Visão")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float keyboardRotationSpeed = 50f; // Velocidade para o WASD
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
        _inputActions = new PlayerControls();

        _inputActions.Player.Look.performed += ctx => _rotationInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Look.canceled += ctx => _rotationInput = Vector2.zero;

        _inputActions.Player.RightClick.performed += ctx => _isRightClicking = true;
        _inputActions.Player.RightClick.canceled += ctx => _isRightClicking = false;
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();

    void LateUpdate()
    {
        RotateLook();
    }

    private void RotateLook()
    {
        // 1. Identifica qual dispositivo está a ser usado
        var device = _inputActions.Player.Look.activeControl?.device;
        if (device == null) return;

        float lookX = 0f;
        float lookY = 0f;

        // 2. Lógica para MOUSE
        if (device is Mouse)
        {
            if (!_isRightClicking) return; // Só roda se estiver a clicar

            lookX = _rotationInput.x * mouseSensitivity;
            lookY = _rotationInput.y * mouseSensitivity;
        }
        // 3. Lógica para TECLADO (WASD / Setas)
        else if (device is Keyboard)
        {
            // Usamos Time.deltaTime para a rotação ser suave e igual em qualquer PC
            lookX = _rotationInput.x * keyboardRotationSpeed * Time.deltaTime;
            lookY = _rotationInput.y * keyboardRotationSpeed * Time.deltaTime;
        }

        // Aplicar a rotação Horizontal (No corpo do Player)
        transform.Rotate(Vector3.up * lookX);

        // Aplicar a rotação Vertical (Na Câmera)
        _verticalRotation -= lookY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, maxDownAngle, maxUpAngle);

        playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }
}