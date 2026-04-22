using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI characterNameText; // Nome do personagem
    [SerializeField] private TextMeshProUGUI dialogueText;      // Texto da fala
    [SerializeField] private Button btnPrevious;
    [SerializeField] private Button btnNext;
    [SerializeField] private Canvas canvas;

    private Camera mainCamera;

    private System.Action onNext;
    private System.Action onPrevious;

    void Awake()
    {
        mainCamera = Camera.main;

        btnNext.onClick.AddListener(() => onNext?.Invoke());
        btnPrevious.onClick.AddListener(() => onPrevious?.Invoke());
    }

    void LateUpdate()
    {
        // Faz a UI olhar pra câmera (billboard)
        if (mainCamera != null)
        {
            transform.forward = mainCamera.transform.forward;
        }
    }

    // 🔹 Atualiza nome + texto
    public void SetDialogue(string characterName, string text)
    {
        characterNameText.text = characterName;
        dialogueText.text = text;
    }

    // 🔹 Define ações dos botões
    public void SetCallbacks(System.Action next, System.Action previous)
    {
        onNext = next;
        onPrevious = previous;
    }

    // 🔹 Liga/desliga botões
    public void SetButtonState(bool hasPrevious, bool hasNext)
    {
        btnPrevious.gameObject.SetActive(hasPrevious);
        btnNext.gameObject.SetActive(hasNext);
    }

    public void Show() => canvas.enabled = true;
    public void Hide() => canvas.enabled = false;
}