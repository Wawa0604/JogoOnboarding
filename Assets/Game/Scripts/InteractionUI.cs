using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("Configuração de Exibição")]
    [SerializeField] private GameObject uiPanel; 

    [Header("UI Elements (Atribuição Manual por Cena)")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image imagemAvatar;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button btnPrevious;
    [SerializeField] private Button btnNext;

    public void SetDialogue(string characterName, string text, Sprite avatarSprite)
    {
        if (characterNameText != null) characterNameText.text = characterName;
        if (dialogueText != null) dialogueText.text = text;
        if (imagemAvatar != null) imagemAvatar.sprite = avatarSprite;
    }

    public void SetButtonState(bool hasPrevious, bool hasNext)
    {
        if (btnPrevious != null) btnPrevious.interactable = hasPrevious;
        if (btnNext != null) btnNext.interactable = hasNext;
    }

    public void Show() 
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
        }
    }

    public void Hide() => uiPanel?.SetActive(false);
}