using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractive : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogoParaDisparar;
    [SerializeField] private GameObject seta;

    private PlayerControls _inputActions;

    void Awake()
    {
        // Inicializa a classe baseada no ficheiro da tua imagem
        _inputActions = new PlayerControls();
        
        // Subscreve a ação de clique esquerdo (aquela que vais criar no Passo 1)
        _inputActions.Player.Click.performed += ctx => DetectarAlvo();
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();

    private void DetectarAlvo()
    {
        // 1. Pega a posição do rato no ecrã (Novo Sistema)
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        
        // 2. Lança um raio da câmara para o mundo
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        // 3. Verifica se bateu NESTE NPC
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Interact();
        }
    }

    public void Interact()

    {
        if (seta != null) seta.SetActive(false);

        // Se a Instance estiver nula, buscamos o Manager da cena atual
        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("Manager não encontrado pela instância, buscando na hierarquia...");
            
            // MUDANÇA AQUI: Trocamos FindFirst por FindAny para remover o warning
            var managerNaCena = Object.FindAnyObjectByType<DialogueManager>();
            
            if (managerNaCena != null)
            {
                managerNaCena.IniciarConversa(dialogoParaDisparar);
                return;
            }
        }

        // Caso normal: usa a instância que se auto-atribuiu no Awake do Manager da cena
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.IniciarConversa(dialogoParaDisparar);
        }
        else
        {
            Debug.LogError("ERRO: Não existe um DialogueManager nesta cena para o NPC falar!");
        }
    }
}