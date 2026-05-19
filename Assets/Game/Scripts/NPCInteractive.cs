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
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (seta != null) seta.SetActive(false);

        // Chama o manager configurado localmente na cena atual
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