using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("Configuração de Exibição")]
    [SerializeField] private GameObject uiPanel; // O objeto filho que aparece/some

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button btnPrevious;
    [SerializeField] private Button btnNext;

    private System.Action onNext;
    private System.Action onPrevious;

    void Awake()
    {
        if (btnNext != null)
            btnNext.onClick.AddListener(() => onNext?.Invoke());

        if (btnPrevious != null)
            btnPrevious.onClick.AddListener(() => onPrevious?.Invoke());
    }

    public void SetDialogue(string characterName, string text)
    {
        if (characterNameText != null) characterNameText.text = characterName;
        if (dialogueText != null) dialogueText.text = text;
    }

    public void SetCallbacks(System.Action next, System.Action previous)
    {
        onNext = next;
        onPrevious = previous;
    }

    public void SetButtonState(bool hasPrevious, bool hasNext)
    {
        if (btnPrevious != null) btnPrevious.interactable = hasPrevious;
        if (btnNext != null) btnNext.interactable = hasNext;
    }

    public void Show() => uiPanel?.SetActive(true);
    public void Hide() => uiPanel?.SetActive(false);
}