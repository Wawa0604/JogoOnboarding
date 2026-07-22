using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CollectibleObject : MonoBehaviour
{
    [Header("Receita do Item")]
    public CollectibleData data; // Arraste o seu Scriptable Object aqui!

    private PlayerControls _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerControls();
    }

    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    private void Update()
    {
        // 1. Checa se o jogador clicou neste exato frame
        if (_inputActions.Player.Click.WasPerformedThisFrame())
        {
            DetectarClique();
        }
    }

    private void DetectarClique()
    {
        // 2. Proteção da UI: Se o mouse estiver em cima de um painel, ignora o clique no item
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }

        // 3. Dispara o raio da câmera até a posição do mouse
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 4. Se o raio bater em um COLLIDER 3D (lembre de usar o BoxCollider normal no Sprite)
        if (Physics.Raycast(ray, out hit))
        {
            // 5. Verifica se o objeto que o raio bateu é este coletável
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                AoSerClicado();
            }
        }
    }

    private void AoSerClicado()
    {
        // Mostra a UI na tela (Sempre vai abrir, para o jogador poder reler)
        if (CollectibleUIPopup.Instance != null)
        {
            CollectibleUIPopup.Instance.ExibirItem(data);
        }
        else
        {
            Debug.LogWarning("O painel de UI dos coletáveis não foi encontrado na cena!");
        }

        // Tenta registrar a coleta (o Manager bloqueia automaticamente se já foi pego antes)
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.RegistrarColeta(data.id);
        }
    }
}