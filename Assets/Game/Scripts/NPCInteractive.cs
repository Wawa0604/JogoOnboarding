using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // precisa para parar os clicks que passam a ui

public class NPCInteractive : MonoBehaviour
{
    [Header("Configurações do NPC")]
    [SerializeField] private DialogueSequence dialogoParaDisparar;
    [SerializeField] private GameObject seta;

    private PlayerControls _inputActions;

    void Awake()
    {
        _inputActions = new PlayerControls();
        // A LINHA ABAIXO FOI REMOVIDA PARA EVITAR O ERRO DA UI
        // _inputActions.Player.Click.performed += ctx => DetectarAlvo();
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();

    // ADICIONADO: O Update garante que o clique e a UI estejam sincronizados no mesmo frame
    void Update()
    {
        if (_inputActions.Player.Click.WasPerformedThisFrame())
        {
            DetectarAlvo();
        }
    }

    private void DetectarAlvo()
    {
        // SE O MOUSE ESTIVER EM CIMA DE QUALQUER ELEMENTO DE UI, CANCELA O CLIQUE NO NPC
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
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