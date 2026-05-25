using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractive : MonoBehaviour
{
    [Header("Configurações do NPC")]
    [SerializeField] private DialogueSequence dialogoParaDisparar;
    [SerializeField] private GameObject seta;

    private PlayerControls _inputActions;

    void Awake()
    {
        _inputActions = new PlayerControls();
        _inputActions.Player.Click.performed += ctx => DetectarAlvo();
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();

    private void DetectarAlvo()
    {
        // 1. Pega a posição do mouse na tela
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        // 2. Cria um raio da câmera em direção ao mundo 3D
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
        // 3. Estrutura que vai guardar as informações do que o raio atingir
        RaycastHit hit;

        // 4. Dispara o Raycast usando a física 3D
        if (Physics.Raycast(ray, out hit))
        {
            // Verifica se o colisor atingido pertence a ESTE NPC
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Interact();
            }
        }
    }

    public void Interact()
    {
        if (seta != null) seta.SetActive(false);

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.IniciarConversa(dialogoParaDisparar);
        }
        else
        {
            Debug.LogError("ERRO: Não existe um DialogueManager ativo nesta cena!");
        }
    }
}